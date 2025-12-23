# SiteCrestSolution

A full-stack ASP.NET Core solution for managing chemical pathways and guideline values. The solution includes:

**Backend API ** – Provides CRUD operations for chemicals, pathways, and associated values.

**Frontend** – Blazor-based UI for adding, editing, and viewing chemical values.

**Contracts** – Shared DTOs between frontend and backend for type-safe communication.

**Tests** – Unit tests to ensure data integrity and application reliability.


The application allows users to:

•	Create, view, update, and delete:
o	Chemicals (e.g., Benzene, Toluene)
o	Guideline values associated with:
	One or more exposure pathways (e.g., Direct Soil Contact, Protection of Domestic Use Aquifer)
	Soil texture (Fine or Coarse)

•	Select a chemical
•	Enter a measured concentration value
•	Select:
o	A soil texture (Fine or Coarse)
o	Zero or more exposure pathways (if none are selected, evaluate against all available pathways)
•	View:
o	Each applicable pathway guideline value
o	Whether the measured concentration is above or below each guideline
o	The lowest applicable guideline value for the selected inputs
