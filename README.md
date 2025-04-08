[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Unit Tests](https://github.com/ivanhoe-k/lightweight-ddd/actions/workflows/unit_tests_ci.yml/badge.svg)](https://github.com/ivanhoe-k/lightweight-ddd/actions/workflows/unit_tests_ci.yml)

# Lightweight.DDD – Practical Patterns and Ideas from Real Projects

> 🌟 If you find this helpful, a star would mean a lot!  
> ⚠️ This is not a framework. It’s not a standard. It’s a perspective. A set of ideas shared with curiosity and humility.

## ✨ What This Is

This repository is a small collection of practical design patterns and ideas I’ve found useful while working on real-world Domain-Driven Design (DDD) projects.

I don’t claim to have all the answers. This is just my interpretation — shaped by experience, shaped by constraints — and it may not be universally correct.

These aren’t polished blueprints or definitive solutions. They’re lightweight building blocks — tools that helped me structure, simplify, and evolve code in ways that felt maintainable and expressive.

I don’t expect these patterns to be reused as-is. Instead, I hope they give you a starting point — something to explore, question, or adapt into your own design, based on your domain and needs.

### I believe:
- Patterns work best when **adapted**, not adopted blindly  
- DDD should be **tailored** to the domain, not templated  
- Examples are most valuable when they’re **focused** and **simplified**, not bloated with boilerplate

---

## 📦 Included Projects

| Project                             | Description                                                              |
|-------------------------------------|--------------------------------------------------------------------------|
| `LightweightDdd`              | Core building blocks and reusable DDD abstractions (domain entities, results, events, etc.) |
| `LightweightDdd.Examples.Domain`   | Sample usage with realistic aggregates like `Profile` and its workflows  |
| `LightweightDdd.Tests`             | Unit tests for both Core and Examples, structured by domain context      |

---

## 📁 Structure

### `LightweightDdd`  
| Folder                         | Description                                                        |
|-------------------------------|--------------------------------------------------------------------|
| `Domain/Entity`               | Base abstractions for aggregates and domain entities               |
| `Domain/Errors`               | Domain-specific error definitions and codes                        |
| `Domain/Events`               | Contracts and helpers for domain event dispatching                 |
| `Domain/Repositories`         | Repository interfaces for persistence                              |
| `Domain/Virtualization`       | Virtual Entity Pattern core (hydration, mutation, tracking, etc.)  |
| `Extensions`                  | Guard clauses and expressive domain-safe extensions                |
| `Results`                     | Functional result modeling: `Result<T>`, `Result<TError, TValue>`  |

### `LightweightDdd.Examples.Domain`  
| Folder         | Description                                                       |
|----------------|-------------------------------------------------------------------|
| `Contracts`    | Repository and service abstractions used by the domain            |
| `Errors`       | Domain-specific error types and enums                             |
| `Events`       | Events triggered by aggregates (e.g., `ProfileOnboarded`)         |
| `Models`       | Aggregates and value objects (e.g., `Profile`, `Media`)           |
| `Workflows`    | Domain workflows coordinating logic (e.g., `ProfileWorkflows.cs`) |

### `LightweightDdd.Tests`  
| Folder                          | Description                                                            |
|----------------------------------|------------------------------------------------------------------------|
| `UnitTests/Core/Results`         | Tests for functional result behavior and error flows                   |
| `UnitTests/Core/Virtualization`  | Tests for virtual property lifecycle: hydration, mutation, access      |
| `UnitTests/Examples/Domain`      | Tests for the example domain logic and workflows                       |

---

## 🔍 Included Patterns (So Far)

- ✅ `DomainEntity<TKey>` and `VersionedDomainEntity<TKey>`
- ✅ `IDomainEvent`, `IDomainEntity`, and domain event dispatching contracts
- ✅ `Result<TError, TValue>` and `Result<TError>` for explicit control flow and structured errors
- ✅ Virtual Entity Pattern — safe partial hydration and fine-grained change tracking  
  Includes:
  - `VirtualPropertyBase<TEntity, TProperty, TSelf>`
  - Sealed variants: `VirtualProperty<TEntity, TProperty>`, `NullableVirtualProperty<TEntity, TProperty>`
  - Internal-only `IResolvable` interface for safe builder-only hydration
  - Explicit domain mutation via `Update(...)`
  - Flags: `HasResolved`, `HasChanged` for lifecycle control
  - Typed argument containers and builders (`VirtualArgs`, `VirtualArgsBuilderBase`)
- ✅ Guard extensions for expressive null/default checks
- ✅ Domain-level workflows for orchestrating domain logic in a consistent and technology-agnostic way
- ✅ Read/Write repository split to encourage logical CQRS boundaries in the domain layer

---

## 📅 Example Use Case: Profile + Virtual Entity Pattern

The `LightweightDdd.Examples` project includes a working `Profile` aggregate that demonstrates:

- Immutable Value Objects (`Address`, `PersonalInfo`, `Media`)
- Versioned aggregate with domain events (`ProfileOnboarded`, `AvatarUpdated`, etc.)
- Business rules expressed via methods returning `Result<DomainError>`
- Support for partial hydration through `VirtualProfile`
- Guarded property access with `VirtualProperty<T>`, throwing on unresolved fields
- Fluent args builder (`VirtualProfileArgsBuilder`) for type-safe resolution
- Support for detecting mutations without full entity tracking (`HasChanged`)

This example shows how to avoid over-fetching, prevent silent logic errors, and still maintain rich domain behavior — even when dealing with partial state.

### 🔄 Hypothetical Usage

```csharp
// Simulate a projection for a business case: verifying a profile
var projection = new
{
    Id = Guid.NewGuid(),
    Version = 3,
    Verification = VerificationStatus.Pending
};

// Build only what we need for this operation
var args = VirtualProfileArgs
    .GetBuilder()
    .WithVerification(projection.Verification)
    .Build();

// Create a virtual version of the entity
var result = Profile.CreateVirtual(projection.Id, projection.Version, args);

if (result.Failed)
{
    // Handle invalid ID/version/etc.
    return;
}

var virtualProfile = result.Value;

// Domain logic succeeds
var verifyResult = virtualProfile.Verify();

// But accessing something unresolved throws:
var name = virtualProfile.PersonalInfo.FullName;
// ↑ Throws VirtualPropertyAccessException since PersonalInfo wasn't resolved
```
---

## ❤️ Why I’m Sharing This

Because I’ve been there:
- Trying to follow DDD while struggling to balance **purity with practicality**
- Seeing frameworks that **overcomplicate simple problems**
- Finding little tricks that solved **big headaches**

If anything here helps you think more clearly, ship more confidently, or discuss DDD more deeply — then it was worth publishing.

Please feel free to fork, adapt, evolve, simplify, or challenge what you find here.

---

### ⚙️ What Might Be Coming Next (Experimental Ideas)

While Lightweight.Ddd is intentionally minimal, there are a couple of directions I’m exploring based on real-world  challenges I’ve encountered while working on Domain-Driven projects, both in personal explorations and commercial environments.

#### 🚦 1. Railway-Oriented Result Extensions

One potential area of improvement is a lightweight, fluent API for `Result<T>` and `Result<TError, TValue>`, focused on expressive success/failure pipelines.

Planned additions include:

| Method           | Purpose                                                                 |
|------------------|-------------------------------------------------------------------------|
| `Then`           | Chain monadic operations (`Result<T> -> Result<U>`)                     |
| `Map`            | Transform the success value (`T -> U`), preserving structure            |
| `Catch`          | Handle or react to failure without changing the result                  |
| `CatchIf`        | Handle failures conditionally based on error predicate                 |
| `Tap`            | Side-effect on success (e.g., logging)                                  |
| `TapFailure`     | Side-effect on failure                                                  |
| `Ensure`         | Enforce domain invariants — convert success into failure                |
| `Match`          | Terminal: exhaustively handle both branches and return raw value       |

Design goals:
- Async support via overloads (no `Async` postfix)
- Terse and idiomatic naming (`Then`, `Tap`, etc.)
- Designed for real-world domain logic, not academic monad purity

### 🌱 2. Domain Effects (Async Side Effects with Boundaries)

I’ve been experimenting with a clean domain-level model for **Domain Effects**:
- Effects are not raised directly by aggregates
- Instead, they are often produced by **domain event handlers**, allowing flexible many-to-many relationships between events and effects
- They represent **meaningful async operations** that happen as a result of domain behavior (e.g., sending notifications, scheduling delayed domain actions, background image processing, or publishing integration events)

In some projects, I’ve implemented this with:
- A **transactional outbox** to persist effects alongside the aggregate
- A **workflow** that:
  1. Retrieves batches of effects (prioritized)
  2. Resolves effect handlers
  3. Applies them (in parallel if needed)
  4. Updates their statuses (e.g. success/failure)

What’s important is that this design lives entirely in the **domain layer**, relying only on clean **ports/abstractions**.  
While it includes orchestration logic (e.g., batching, retries, resolution), these operations are delegated to injected services — keeping the core workflow free from infrastructure concerns and easily adaptable across different persistence or messaging implementations.

✅ This approach has proven to be **reliable in both monolithic and distributed (microservices) architectures**.  
In monoliths, domain effect handlers may perform the operations directly.  
In service-based architectures, they can trigger **integration events** or enqueue work — without compromising consistency.

> 📝 These ideas are already implemented in some of my domain-driven projects where effects are persisted transactionally, prioritized based on domain-defined importance, and handled via clean abstractions.  
> I don’t yet know if I’ll have the time or energy to extract and adapt these components for this toolkit — but if I do, they could serve as a solid foundation for modeling domain effects and enabling reliable async workflows across bounded contexts.

If extracted, this might take the form of:
- A clean **domain effect abstraction**
- A **base class** relying only on ports
- Sample orchestration patterns

---

## 🙏 A Small Ask

This project isn't meant to be dropped in as-is or treated as a framework.

It's a collection of ideas — building blocks you're free to **copy, adapt, and shape** to fit your domain.  
If anything here helped you think more clearly or saved you a few hours of design frustration...

⭐ **Please consider starring the repo** to show your support.  
That’s the best way to let me know this work has been useful — and it genuinely helps motivate me to keep sharing more.

Thanks again for reading, exploring, and thinking through these ideas.

---

## ⚙️ Requirements

- .NET 9
- Visual Studio 2022+ or JetBrains Rider

---

## 📝 License

Licensed under the [MIT License](LICENSE).  
Use freely. No strings attached.