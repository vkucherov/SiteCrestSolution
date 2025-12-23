FRONT - https://sitecrestfront-hmh4cbffc8brdkcd.canadacentral-01.azurewebsites.net/

Back - https://sitecrestbackend-h5avehf4baaabfez.canadacentral-01.azurewebsites.net/

# SiteCrestSolution

A full-stack ASP.NET Core solution for managing chemical pathways and guideline values. The solution includes:

**Backend API ** – Provides CRUD operations for chemicals, pathways, and associated values.

**Frontend** – Blazor-based UI for adding, editing, and viewing chemical values.

**Contracts** – Shared DTOs between frontend and backend for type-safe communication.

**Tests** – Unit tests to ensure data integrity and application reliability.


## 1. Chemicals API

| Endpoint           | Method | Description |
|------------------|--------|-------------|
| `/chemicals`      | GET    | Get all chemicals (with their pathway values) |
| `/chemicals/{id}` | GET    | Get a specific chemical by ID (includes all pathway values) |
| `/chemicals`      | POST   | Create a new chemical. Body example: <br>```json { "name": "ChemicalName" }``` |
| `/chemicals/{id}` | PUT    | Update a chemical's name. Body example: <br>```json { "chemicalName": "NewName" }``` |
| `/chemicals/{id}` | DELETE | Delete a chemical by ID |

**Notes:**  
- POST/PUT requests return `400` if required fields are missing.  
- POST returns `409` if a chemical with the same name exists.  

---

## 2. Pathways API

| Endpoint           | Method | Description |
|------------------|--------|-------------|
| `/pathways`      | GET    | Get all pathways (sorted by `SortValue`) |
| `/pathways/{id}` | GET    | Get a specific pathway by ID |

---

## 3. Values API (Chemical-Pathway)

| Endpoint | Method | Description |
|----------|--------|-------------|
| `/values` | POST | Add or update chemical-pathway values. Body example: <br>```json { "chemicalId": 1, "values": [ { "pathwayId": 2, "value": 50.0 }, { "pathwayId": 3, "value": 25.5 } ] }``` |

**Notes:**  
- Returns `400` if any value is negative.  
- Updates existing values if the combination already exists.  
- Returns `404` if the chemical does not exist.  

---

## 4. CORS

Frontend is allowed to access the API from:  

- `https://localhost:7095`  
- `https://sitecrestfront-hmh4cbffc8brdkcd.canadacentral-01.azurewebsites.net`  

---

## 5. Database

- **Testing environment:** In-memory DB (`AppTestsDb`)  
- **Development/Production:** SQL Server (configured via `appsettings.json`)  

---
