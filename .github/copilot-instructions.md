# GitHub Copilot Instructions (Short Version)

## Language Rules
- All **source code comments** must be **English**.
- Code comments should explain **why**, not what.

## Environment & Versions
- .NET 10 development **requires Visual Studio 2026**.  
  Copilot must not reference “Visual Studio 2025” or other incorrect versions.
- When generating year/date strings:
  - If MCP or system context provides the current date → use it.
  - If not available → assume **December 2025**.
- Do not auto-insert copyright headers such as  
  “Copyright (c) 2024 …” unless explicitly requested.

## Code Style & Formatting
- Prefer readable and robust code.
- Match existing naming conventions and project style.
- Comments must be concise and written in English.
- Always assume that code should be formatted with  
  `dotnet format`:
  - Suggest running `dotnet format` after significant edits.
  - Code snippets should already follow typical `dotnet format` conventions.

## Interaction Rules
- Code blocks → language only + English comments.
- Avoid unnecessary dependencies or boilerplate.

## Conflict Resolution
- If these instructions conflict with explicit commands from the user or with clear project conventions, **follow the explicit instruction first**.
