
# Problem Domain Description
My application is a system to track auto parts at stores for comparison shopping.

- The client would be a third-party service provider (maybe a website) - not the stores or the customers.

- The application is a system to track auto parts at different stores to
  allow customers to find parts they’re looking for at the cheapest price.
  Stores would have different amounts of inventory, and each store would have
  their own price for an item. The application would also track orders by customers.

- This is a bit unrealistic, but the users will only be the customers shopping for parts.
  Ideally, there would be clerks at stores who update inventories and whatnot, but I won’t implement that now.


## Core Entities and Relationships

### Entities

The items in the database will be:

- Customers - name, email address
- Items - id, description, manufacturer's name
- Stores - id, name, address
- Inventories at a Store - item id, store id, quantity, price
- Orders - id, customer id, store id, total amount
- Order Items id, order id, item id, quantity, total amount


### Relationships

- Stores have Inventory of Items - A Store has many Inventories (1:M). An
  Item in Inventory of many Stores (1:M). These create a many-to-many (M:M)
  relationship between Stores and Items - many items are in many stores.
- A Customer places many Orders (1:M).
- An Order has mutiple Order Items (1:M).
- An Item can be ordered in multiple Order Items (1:M).


# User Stories

- As a Customer, I want to list available Items, so that I know what I can
  buy.
- As a Customer, I want to search for an Item by prices at all of the
  Stores, so that I can get a deal.
- As a Customer, I want to see all of the Items at a Store, so that I know
  what to look for at that store.
- As a Customer, I want to place an Order for serveral Items, so that I can
  fix my broken-down car.
- As a Customer, I want to search for all the Stores that have a given Item,
  so that I can decide which store to go to.
- As a Customer, I want a list of Stores ordered by distance from some
  location, so that I can know which stores are closest.
- As an employee at a Store, I want to fullfill a Customer's order by
  checking the Store's inventory and decreasing the inventory amounts, so
  that the Customer gets their stuff, and the Store can charge them money.
- As an employee at a Store, I want to update the price or quantity of an
  item that we have in stock, so that the information in the system is
  accurate.

## Business Rules

- A Customer can only place an Order at a Store for Items that are in stock
  positive Inventory quantity.
- All of the Order Items for an Order must be available at the same Store
- Inventory quantities can never be less than zero.
- An Order must have at least one Order Item
- An Order cannot have more than 20 Order Items (so that it fits on the old
  receipt forms)

## Database Operations

- To list available items: Query Items joined with Inventories where
  quantity > 1, order by item name.
- To find lowest price for an item ID: Query Items joined with Inventories where
  ID is the item we're searching for, order by item inventory price
  ascending.
- To update inventory at a store: Query for item ID in inventory at our store.
  If found, is the new quantity greater than 1. If so, update the inventory
  record, otherwise delete it. If there is no inventory record, insert a new
  one with the new quantity.
- To place an order at a store: Query for all items using item IDs and the
  store ID to check available inventory. Insert a new Order. For each item
  in the order, Insert an Order Item and update the quantity on each
  Inventory record.

# Out of Scope

- As mentioned above, the application will not (initially) support a way for
  workers at stores to update the inventory reocrds.
- Although there are Orders, the application will not track payment details
  like credit cards.
- There should be some concept of administrative users to manage the system,
  but we won't model that in the system. The admins can use SQL directly.
- It would be cool (but not implemented) to allow users to watch Items and
  Stores to get an alert when an Item shows up in a Store that the user
  likes to shop at.
- Another unimplemented idea is to restrict the search to a subset of the
  stores based on the Customers's preferences.
