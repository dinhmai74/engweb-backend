new Mongo()
conn = new Mongo()
db = conn.getDB('HollyStoreDb')
db.createCollection('Warehouse')
db.Products.insertOne({
    ProductId: "",
    IsSold: false
})