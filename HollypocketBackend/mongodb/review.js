new Mongo()
conn = new Mongo()
db = conn.getDB('HollyStoreDb')
db.createCollection('Review')
db.Rates.insertMany([
    {
        ValueRating: "1"
    },
    {
        ValueRating: "2"
    },
    {
        ValueRating: "3"
    }
])