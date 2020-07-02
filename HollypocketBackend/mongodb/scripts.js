new Mongo();
conn = new Mongo();
db = conn.getDB("HollyStoreDb");
db.createCollection("Books");
db.Books.insertMany([
  {
    Name: "Design Patterns",
    Price: 54.93,
    Category: "Computers",
    Author: "Ralph Johnson",
  },
  {
    Name: "Clean Code",
    Price: 43.15,
    Category: "Computers",
    Author: "Robert C. Martin",
  },
]);

load("product.js");
load("account.js");
load("question.js");
load("warehouse.js");
load("order.js");
load("cart.js");
load("favorite.js");
load("provider.js");
load("review.js");
