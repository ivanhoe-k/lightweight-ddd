[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

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
| `LightweightDdd.Core`              | Core building blocks and reusable DDD abstractions (domain entities, results, events, etc.) |
| `LightweightDdd.Examples.Domain`   | Sample usage with realistic aggregates like `Profile` and its workflows  |
| `LightweightDdd.Tests`             | Unit tests for both Core and Examples, structured by domain context      |

---

## 📁 Structure

### `LightweightDdd.Core`
| Folder            | Description                                               |
|-------------------|-----------------------------------------------------------|
| `DomainModel`     | Base abstractions for aggregates and entities             |
| `Events`          | Domain event contracts and helper types                   |
| `Results`         | Functional-style result modeling (`Result<T>`, `IError`)  |
| `Virtualization`  | Virtual entity pattern core (e.g. `VirtualProperty<T>`)   |
| `Extensions`      | Guard clauses and other domain-safe utilities             |
| `Repositories`    | Domain-level repository contracts                         |
| `Utilities`       | Shared low-level helpers                                  |

### `LightweightDdd.Examples.Domain`
| Folder         | Description                                                       |
|----------------|-------------------------------------------------------------------|
| `Models`       | Realistic aggregates and value objects (e.g. `Profile`, `Media`)  |
| `Errors`       | Domain errors and error code enums                                |
| `Events`       | Domain events emitted by aggregates                               |
| `Contracts`    | Domain-level ports such as repositories and services              |
| `Workflows`    | Domain workflows (orchestrators) coordinating domain operations   |

---

## 🔍 Included Patterns (So Far)

- ✅ `DomainEntity<TKey>` and `VersionedDomainEntity<TKey>`
- ✅ `IDomainEvent`, `IDomainEntity`, and domain event dispatching contracts
- ✅ `Result<T>`, `IError` for explicit control flow and structured errors
- ✅ Virtual Entity Pattern for safe partial hydration of aggregates  
  (includes `VirtualProfile`, `VirtualProperty<T>`, fluent builder + args model)
- ✅ Guard extensions for expressive null/default checks
- ✅ Domain-level workflows for orchestrating domain logic in a consistent and technology-agnostic way
- ✅ Read/Write repository split to encourage logical CQRS boundaries in the domain layer

> More will be added and refined over time.

---

## 📁 Structure

| Folder            | Description                                     |
|-------------------|-------------------------------------------------|
| `DomainModel`     | Core domain entity abstractions                 |
| `Events`          | Domain events and effects                       |
| `Results`         | Functional-style result modeling (`Result<T>`, `IError`) |
| `Virtualization`  | Tactical pattern for partial entity hydration   |
| `Extensions`      | Guard clauses and helper utilities              |

---

## 🧪 Example Use Case: Profile + Virtual Entity Pattern

The `LightweightDdd.Examples` project includes a working `Profile` aggregate that demonstrates:

- Immutable Value Objects (`Address`, `PersonalInfo`, `Media`)
- Versioned aggregate with domain events (`ProfileOnboarded`, `AvatarUpdated`, etc.)
- Business rules expressed via methods returning `Result<IDomainError>`
- Support for partial hydration through `VirtualProfile`
- Guarded property access with `VirtualProperty<T>`, throwing on unresolved fields
- Fluent args builder (`VirtualProfileArgsBuilder`) for type-safe resolution

This example shows how to avoid over-fetching, prevent silent logic errors, and still maintain rich domain behavior — even when dealing with partial state.

🔧 This is still a work in progress. The example will grow as more patterns and workflows are added.

### 🔄 Hypothetical Usage

Even without a repository yet, here's what a potential usage of `VirtualProfile` might look like in a domain-specific scenario:

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