new Mongo()
conn = new Mongo()
db = conn.getDB('HollyStoreDb')
db.createCollection('Questions')
db.Questions.insertMany([
    {
        Value:"sanphamnhucaiulon"
    },
    {
        Value: "dasdasd"
    },
    {
        Value: "qdasdasdqwfxc"
    }
])