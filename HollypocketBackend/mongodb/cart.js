new Mongo()
conn = new Mongo()
db = conn.getDB('HollyStoreDb')
db.createCollection('Carts')
db.Carts.insertOne(
    {
        UserId: "123abc",
        Product: [{
            ProductId: "123abc",
            Amount: 1
        }]
    }
)