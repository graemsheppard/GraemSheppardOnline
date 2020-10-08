using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MongoDB.Driver.Linq;
using System;

namespace GraemSheppardOnline.Services.MongoDBContext
{
    public class ItemCollection<T> : ContextCollection<T>
    {

        public ItemCollection(IMongoCollection<T> col) : base(col) { }

        /// <summary>
        /// Finds the first parent where the specified field exists and returns it.
        /// Currently cannot search for nested fields.
        /// </summary>
        /// <typeparam name="T1">The type of object to return</typeparam>
        /// <param name="id">Id of the start document</param>
        /// <param name="itemField">Field to search for</param>
        /// <returns>First non-null value of the specified field</returns>
        public T1 FirstResultFor<T1>(string id, ItemField itemField)
        {
            
            var field = itemField.ToString();
            T1 temp = default;

            #region pipeline
            var pipeline = new[]
            {

                new BsonDocument("$match",
                new BsonDocument("_id",
                new ObjectId(id))),
                new BsonDocument("$graphLookup",
                new BsonDocument
                {
                    { "from", "Items" },
                    { "startWith", "$_id" },
                    { "connectFromField", "ParentId" },
                    { "connectToField", "_id" },
                    { "as", "Parents" },
                    { "depthField", "Depth" }
                }),
                new BsonDocument("$unwind",
                new BsonDocument("path", "$Parents")),
                new BsonDocument("$replaceRoot",
                new BsonDocument("newRoot", "$Parents")),
                new BsonDocument("$sort",
                new BsonDocument("Depth", 1)),
                new BsonDocument("$match",
                new BsonDocument(field,
                new BsonDocument("$ne", BsonNull.Value)))
            };
            #endregion

            var result = Collection.Aggregate<Item>(pipeline).FirstOrDefault();
            if (result == null) { return temp; }
            var prop = typeof(Item).GetProperty(field);
            temp = (T1)prop?.GetValue(result);
            return temp;

        }

        public List<T1> GetAncestry<T1>(string id, ItemField itemField)
        {
            string field = itemField.ToString();
            List<T1> temp = null;
            #region pipeline
            var pipeline = new[]
            {

                new BsonDocument("$match",
                new BsonDocument("_id",
                new ObjectId(id))),
                new BsonDocument("$graphLookup",
                new BsonDocument
                {
                    { "from", "Items" },
                    { "startWith", "$_id" },
                    { "connectFromField", "ParentId" },
                    { "connectToField", "_id" },
                    { "as", "Parents" },
                    { "depthField", "Depth" }
                }),
                new BsonDocument("$unwind",
                new BsonDocument("path", "$Parents")),
                new BsonDocument("$replaceRoot",
                new BsonDocument("newRoot", "$Parents")),
                new BsonDocument("$sort",
                new BsonDocument("Depth", 1))
            };
            #endregion

            var result = Collection.Aggregate<Item>(pipeline).ToEnumerable();

            var prop = typeof(Item).GetProperty(field);
            temp = result.Select(x => (T1)prop.GetValue(x)).ToList();

            return temp;
        }

        public List<T1> Accummulate<T1>(string id, ItemField itemField)
        {
            string field = itemField.ToString();
            List<T1> temp = null;
            #region pipeline
            var pipeline = new[]
            {

                new BsonDocument("$match",
                new BsonDocument("_id",
                new ObjectId(id))),
                new BsonDocument("$graphLookup",
                new BsonDocument
                {
                    { "from", "Items" },
                    { "startWith", "$_id" },
                    { "connectFromField", "ParentId" },
                    { "connectToField", "_id" },
                    { "as", "Parents" },
                    { "depthField", "Depth" }
                }),
                new BsonDocument("$unwind",
                new BsonDocument("path", "$Parents")),
                new BsonDocument("$replaceRoot",
                new BsonDocument("newRoot", "$Parents")),
                new BsonDocument("$sort",
                new BsonDocument("Depth", 1))
            };
            #endregion

            var result = Collection.Aggregate<Item>(pipeline).ToEnumerable();
            
            var prop = typeof(Item).GetProperty(field);


            var lists = from t1 in result
                        select (IEnumerable<T1>)prop.GetValue(t1);

            var query = from t1 in lists
                        where t1 != null
                        from t2 in t1
                        select t2;

            temp = query.ToList();
            return temp; 
        }

        public TOut TestMethod<TOut>(Func<Item, TOut> func, Item item)
        {
            return func(item);
        }

    }


    public enum ItemField
    {
        PriceId,
        Nested,
        Alphabet,
    }

    
}
