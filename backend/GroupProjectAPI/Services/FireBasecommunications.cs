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
            Console.WriteLine(JsonConvert.SerializeObject(obj));
            Console.WriteLine("IN SAVE TO FS");

            DocumentReference docRef = db
                .Collection("users")
                .Document(uid)
                .Collection(className)
                .Document(documentName);



            // Save the basic fields like DisplayName and WeeklyGoal
            if (obj is UserInfo userInfo)
            {
                await docRef.SetAsync(new
                {
                    userInfo.DisplayName,
                    userInfo.WeeklyGoal
                });
                // Save UserAvailability to a subcollection called "UserAvailability"
                if (userInfo.UserAvailability != null)
                {
                    await SaveToFirestore(userInfo.UserAvailability, uid, "UserAvailability", "availability");
                    await SaveToFirestore(userInfo.UserAvailability.weeklySchedule, uid, "TimeBlock", "time");
                }

                // Save InviteInbox to a subcollection called "InviteInbox"
                // if (userInfo.InviteInbox != null)
                // {
                //     var inviteCollection = db.Collection("users")
                //         .Document(uid)
                //         .Collection("InviteInbox");

                //     foreach (var invite in userInfo.InviteInbox)
                //     {
                //         var inviteDocRef = inviteCollection.Document();
                //         await inviteDocRef.SetAsync(invite);
                //     }
                // }

                // If you have other nested objects like TaskList, PreferredTimes, etc., save them in similar fashion
                // if (userInfo.TaskList != null)
                // {
                //     await SaveToFirestore(userInfo.TaskList, uid, "TaskList", "taskList");
                // }

                // if (userInfo.PreferredTimes != null)
                // {
                //     await SaveToFirestore(userInfo.PreferredTimes, uid, "PreferredTimes", "preferredTimes");
                // }
            }
            else
            {
                await docRef.SetAsync(obj);
            }
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


        /// <summary>
        /// gets the dummy object from firestore - used for testing purposes
        /// </summary>
        /// <param name="uid"> fake user id</param>
        /// <returns></returns>
        public static async Task<Dummy> LoadDummyFromFirestore(string uid)
        {
            DocumentReference docRef = db
                .Collection("users")
                .Document(uid)
                .Collection("dummy")
                .Document("primary");

            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
            if (snapshot.Exists)
            {
                return snapshot.ConvertTo<Dummy>();
            }
            return null;
        }


        /// <summary>
        /// saves the dummy object to firestore - used for testing purposes
        /// </summary>
        /// <param name="uid">fake user id</param>
        /// <param name="dummy"></param>
        /// <returns></returns>
        public static async Task SaveDummyToFirestore(string uid, Dummy dummy)
        {
            DocumentReference docRef = db
                .Collection("users")
                .Document(uid)
                .Collection("dummy")
                .Document("primary");

            await docRef.SetAsync(dummy);
        }
    }
}