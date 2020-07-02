new Mongo()
conn = new Mongo()
db = conn.getDB('HollyStoreDb')
db.createCollection('Favorite')
db.Carts.insertOne(
    {
        UserId: "123abc",
        Product: [{
            ProductId: "123abc"   
        }]
    }
)