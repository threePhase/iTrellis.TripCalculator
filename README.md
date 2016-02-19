# Trip Calculator #

### Overview ###

*Trip Calculator* is a demo project which exposes an API which calculates the
expenses of a group. The calculation works by splitting the total cost evenly
amongst each individual in the group and then determining payment between
individuals based on how much more or less (than the determined split) an
individual spent while on a trip.

### URL Format ###

The following format is used for all API calls:
`api/transactions/(id)` or `api/calculate`

- Calculate

### Transactions ###
**endpoint**: `api/transactions`

**Transactions** are used for tracking expenses and are the main object used
when manipulating the API. Transactions have the following properties with
required properties in bold.

Field|Description
-----|-----------
**Id** | The transaction's unique id
**Amount** | Expense incurred by transaction
**Owner** | Individual who paid for expense

The following functions are exposed for transactions:

- Add Transaction - **POST** `api/transactions`
- Update Transaction **PUT** `api/transactions/{id:int}`
- Remove Transaction **DELETE** `api/transactions/{id:int}`
- Read Transaction - **GET** `api/transactions/{id:int}`
- Read all Transaction owned by a given **owner**- **GET** `api/transactions/{owner:string}`
- Read all Transactions - **GET** `api/transactions`

For example, a transaction can be retrieved by sending a GET request to:
 `api/transactions/{id:int}` where `{id:int}` is a valid int of the id of the
transaction you wish you view.

### Calculate ###
**endpoint**: `api/calculate`

Calculates the appropriate split based on all currently available transactions.

Unlike transactions, Calculate only exposes a single function at the top-level
of the endpoint: 

- Calculate Split - **GET** `api/calculate`

### Demo ###

There is a simple web interface located in iTrellis.TripCalculator/index.html
and iTrellis.TripCalculator/script.js. This page demonstrates functionality
of each of the API functions.

To run the sample, simply begin debugging the project (by pressing F5 after
opening the project in Visual Studio). After starting the project a sample
database will be created on LocalDb which is seeded with some test data. Switch
to Release config to prevent database from being seeded with test data.
