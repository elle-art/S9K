using backend.models;
using Backend.Models;
using Google.Cloud.Firestore;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Backend.Services
{
    public static class FirebaseCommunications
    {
        private static readonly FirestoreDb db = FirestoreDb.Create("the-scheduler-9000");

        /// <summary>
        /// Saves an object to firestore
        /// </summary>
        /// <typeparam name="T"> object to be saved</typeparam>
        /// <param name="uid">the user's id</param>
        /// <param name="className">the name of the class</param>
        /// <param name="documentName">the name of the file the object is saved under</param>
        /// <param name="obj">the object being saved</param>
        /// <returns></returns>
        public static async Task SaveToFirestore<T>(T obj, string uid, string className, string documentName = "primary")
        {
            DocumentReference docRef = db
                .Collection("users")
                .Document(uid)
                .Collection(className)
                .Document(documentName);

            await docRef.SetAsync(obj);
        }

        /// <summary>
        /// Loads an object from Firestore
        /// </summary>
        /// <typeparam name="T">object to be loaded</typeparam>
        /// <param name="uid">the user's id</param>
        /// <param name="className">the name of the class</param>
        /// <param name="documentName">the name of the file the object is being saved under</param>
        /// <returns></returns>
        public static async Task<T> LoadFromFirestore<T>(string uid, string className, string documentName = "primary")
        {
            DocumentReference docRef = db
                .Collection("users")
                .Document(uid)
                .Collection(className)
                .Document(documentName);

            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
            if (snapshot.Exists)
            {
                return snapshot.ConvertTo<T>();
            }
            return default;
        }
    }
}