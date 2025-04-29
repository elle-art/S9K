using Backend.Models;

namespace Backend.Services
{
    public class DBCommunications
    {

        public static async Task SaveDummyAsync(string uid, Dummy dummy)
        {
            await FirebaseCommunications.SaveDummyToFirestore(uid, dummy);
        }

        public static async Task<Dummy> GetDummyAsync(string uid)
        {
            return await FirebaseCommunications.LoadDummyFromFirestore(uid);
        }

        /// <summary>
        /// saves an object to firestore
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uid">the user id</param>
        /// <param name="className">the class name of the object. Case sensitive</param>
        /// <param name="obj">the object to be saved</param>
        /// <returns></returns>
        public static async Task SaveObjectAsync<T>(string uid, string className, T obj)
        {
            await FirebaseCommunications.SaveToFirestore(obj, uid, className);
        }

        /// <summary>
        /// retrieves an object from firestore
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uid">the user's id</param>
        /// <param name="className">the name of the class</param>
        /// <returns></returns>
        public static async Task<T> GetObjectAsync<T>(string uid, string className)
        {
            return await FirebaseCommunications.LoadFromFirestore<T>(uid, className, "primary");
        }

    }
}