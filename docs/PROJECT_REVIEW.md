# StoErp — Project Review & Handoff Context

> Snapshot review to resume later. No code was changed.
> Reviewed: full `src/` tree (Auth module + Shared), DI, migrations, config.

## 1. Project overview

- **Stack**: .NET 10 (`net10.0`), ASP.NET Core Web API, EF Core 10 + Npgsql (PostgreSQL),
  snake_case naming, `Mediator` (source-generated, martinothamar) instead of MediatR.
- **Architecture intent**: Modular monolith + DDD + CQRS, Clean Architecture layering per module.
- **Format**: New `.slnx` solution file.
- **Modules so far**: `Auth` only; `Emailing` folder is empty (stub in `.slnx`).
- **Shared**: split into `Shared.Application` (events/outbox abstractions),
  `Shared.Contracts` (integration event records), `Shared.Infrastructure` (outbox persistence, event bus).

### Layer/project graph (as referenced)
```
Auth.Domain            -> (nothing)
Auth.Application       -> Auth.Domain, Shared.Application, Shared.Contracts, Shared.Infrastructure  <-- suspect
Auth.Infrastructure    -> Auth.Application, Auth.Domain, Shared.Infrastructure
Shared.Contracts       -> Shared.Application
Shared.Infrastructure  -> Shared.Application, Shared.Contracts
StoErp.Api             -> Auth.Application, Auth.Infrastructure
```
`Auth.Application -> Shared.Infrastructure` is a layering smell (Application should not depend on Infrastructure).

## 2. What the code does today

Feature set (Auth):
- `RegisterUser` — create user + verification token, write outbox `UserRegisteredEvent`, commit.
- `VerifyUser` — validate token, mark used, mark user verified.
- `Login` — detect email vs username in controller, dispatch `LoginWithEmail/Username`,
  shared `LoginService` issues JWT access token + refresh token (hashed) and persists it.
- Transactional outbox: message saved in the **same** `SaveChanges` as the aggregate (correct),
  background `OutboxProcessor` (PeriodicTimer, 5s, batch 50) dispatches via `OutboxDispatcher`,
  strategy pattern (de)serializes per event type, retry w/ exponential backoff (cap 300s),
  dead-letter after 5 attempts, error history JSON.

## 3. What's genuinely good

- Correct **domain isolation** (Domain project has zero deps).
- Value objects as `readonly record struct` + private ctor + static `From()` validation
  (`Email`, `Username`, `PasswordHash`, `UserId`, ...). Proper DDD primitives.
- `Guid.CreateVersion7()` (UUIDv7) for IDs — good index locality.
- **Real** CQRS split: read repos with `AsNoTracking()` + projection DTOs (`UserLoginData`),
  write repos returning aggregates. Not cargo-cult.
- Transactional **outbox** with retry/dead-letter/error-history is advanced, mostly correct.
- EF `IEntityTypeConfiguration` classes + value conversions, separated properly.
- DI extension methods per layer; reflection-based strategy registration.
- Nullable + ImplicitUsings enabled across projects; classes `sealed`.

## 4. Issues (prioritized)

### Correctness / security (high)
1. `VerificationTokenGenerator` uses `new Random()` — **not cryptographically secure**; 6-digit
   numeric code is brute-forceable and `/verify` has no rate limiting. Use `RandomNumberGenerator`.
   (`.../Security/VerificationTokenGenerator.cs`)
2. `InProcessEventPublisher.PublishAsync` **throws when no handler is registered** — with no
   `UserRegisteredEvent` consumer yet, every registration's outbox row retries then dead-letters.
   Should no-op (or log) when there are no subscribers. (`.../Messaging/EventBus/InProcessEventPublisher.cs:23`)
3. Secrets committed: JWT secret + DB password in `src/StoErp.Api/appsettings.json` (tracked in git).
4. `LoginService` **hardcodes** `TimeSpan.FromHours(1)` and `AddDays(7)` while injecting an unused
   `TokenLifeTimeOptions` (config says 15 min). Dead dependency + wrong TTL.
   (`.../Commands/Login/LoginService.cs:54,58`)
5. `Update`/`VerifyUser` are check-then-act with no transaction/lock despite `...ForUpdateAsync`
   naming. `GetByEmailForUpdateAsync` does **not** lock. Race conditions (double verify/token reuse).

### Architecture / design (medium)
6. `Auth.Application` references `Shared.Infrastructure` (unused) — layering violation.
7. Exceptions-as-control-flow everywhere (`InvalidOperationException`); no `Result`, no custom
   exception types, no global exception middleware / ProblemDetails → all business errors return 500.
   Empty `Application/Common/Errors` folder hints this was planned.
8. CQRS blurred: `VerifyUserCommandHandler` uses `IUserReadRepository.GetByEmailForUpdateAsync`
   (read repo) to load a mutable aggregate; read repo also exposes domain `User`.
9. `User` aggregate **encapsulation broken**: `public List<RefreshToken> _refreshTokens = []` and
   `_verificationTokens` are public mutable fields; getters allocate via `.ToList().AsReadOnly()`.
   Tokens are added directly via repositories, bypassing the aggregate root.
10. Application layer carries `JsonDocument` (`OutboxEntry.Payload`) — infra concern leaking up.
11. `OutboxDispatcher.PublishAsync` uses reflection (`MakeGenericMethod`) to call generic publish;
    `DispatchPendingMessagesAsync` is dead code that throws `NotImplementedException`.
12. Options bound manually via `configuration["..."]` + `double.Parse/int.Parse/bool.Parse`
    (culture-sensitive, unvalidated) instead of the Options pattern (`IOptions<T>` + validation).
13. `OutboxMessage.Payload`/`ErrorHistory` hold `JsonDocument` (IDisposable) as long-lived entity
    state — pooled-memory anti-pattern; prefer string + custom converter.
14. `OutboxProcessor` singleton, but `IOutboxReader`s are scoped; correctness depends on scope/reader
    fan-out ordering. Multi-module fan-out logic lives in shared dispatcher — acceptable but note it.
15. No clock abstraction (`DateTimeOffset.UtcNow` scattered) → non-deterministic tests.

### Code quality / consistency (low)
16. `AuthController.Login` duplicates `Email.From` validation inline.
17. API discards command results (`RegisterUser` → `Ok()`); leaks application DTO (`LoginResult`)
    directly as HTTP response instead of API response models.
18. `PasswordHasher`/`RefreshTokenHasher` `new PasswordHasher<object>()` in ctor instead of DI.
19. `RefreshTokenGenerator`: `Guid.NewGuid()` base64 then `.Replace("=","").Replace("+","")` —
    clumsy/lossy entropy handling; only strips `+`, not `/`.
20. Inconsistent DI: `RegisterUserCommandHandler` registered explicitly, others rely on discovery.
21. Migration churn: duplicate pairs (`AddOutboxDeadStateAndErrorHistory` + `...SchemaChange`,
    `VerifiedFlagOnUser` + `VerifiedFlagOnUserV1`) — rebuilt migrations, not cleaned.
22. Block-scoped namespace in `AuthController` vs file-scoped elsewhere; stray empty `<Folder Include>`.
23. No `IDateTimeProvider`, no pipeline behaviors/validation (FluentValidation), no logging in handlers.
24. **No tests at all** (no test projects). Major gap for a DDD/CQRS codebase.
25. No health checks, API versioning, CORS, rate limiting, `[Authorize]` coverage yet.

## 5. Level estimate

**Verdict: upper-junior / early-intermediate (aspiring intermediate).**

- **Architecture & structure: intermediate.** pattern selection (outbox, CQRS split, value objects,
  UUIDv7, slnx, source-gen mediator) reflects heavy study of advanced references.
- **Execution, correctness, security, encapsulation, testing: junior.** insecure RNG, secrets in
  source, unused/hardcoded options, exceptions-as-flow, broken aggregate encapsulation, no tests,
  migration churn, reflection hacks, layering violation.
- Commit messages ("9 hours and i couldn't finish a single handler", "FINALLY STARTING TO GET THE
  HANG OF THIS SHIT") confirm a motivated self-taught learner early on the DDD path.

Scorecard (subjective): Structure 8/10 · Organization 7.5/10 · Correctness 5/10 · Security 4/10 ·
Code quality 6/10. **Overall ~6/10.**

## 6. Suggested next steps (when we resume)

1. Fix insecure verification RNG + add rate limiting; move secrets to user-secrets/env.
2. Wire `TokenLifeTimeOptions`; remove hardcoded TTLs.
3. Make `InProcessEventPublisher` a no-op (or log) when no handlers; remove dead reflection path.
4. Introduce `Result`/exception mapping + global middleware (ProblemDetails).
5. Encapsulate `User` collections; add domain methods (`AddRefreshToken`, `AddVerificationToken`).
6. Remove `Auth.Application -> Shared.Infrastructure`; move `JsonDocument` off `OutboxEntry`.
7. Adopt Options pattern (`IOptions<T>` + validation) for all config.
8. Add a test project (domain unit tests + handler tests + outbox integration test).
9. Clean up migrations; add clock abstraction.

## 7. How to run (as configured)
- API: `dotnet run --project src/StoErp.Api` (Postgres expected at `localhost:5433`, db `stoerp`).
- Migrations: EF Core tooling in `Auth.Infrastructure` (design package present).
