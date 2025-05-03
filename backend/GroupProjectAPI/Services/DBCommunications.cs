using Backend.Models;

namespace Backend.Services
{
    public class DBCommunications
    {
        /// <summary>
        /// saves an object to firebase
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uid">the user id</param>
        /// <param name="className">the class name of the object. Case insensitive</param>
        /// <param name="obj">the object to be saved</param>
        /// <returns></returns>
        public static async Task SaveObjectAsync<T>(string uid, string className, T obj)
        {
            className = className.ToLower();
            await FirebaseCommunications.SaveToFirestore(obj, uid, className);
        }

        /// <summary>
        /// Saves an object to firebase
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uid"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static async Task SaveObjectAsync<T>(string uid, T obj) 
        {
            string className = typeof(T).Name.ToLower();
            await FirebaseCommunications.SaveToFirestore(obj, uid, className);

        }

        /// <summary>
        /// retrieves an object from firestore
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uid">the user's id</param>
        /// <param name="className">the name of the class. Case insensitive</param>
        /// <returns></returns>
        public static async Task<T> GetObjectAsync<T>(string uid, string className)
        {
            className = className.ToLower();
            return await FirebaseCommunications.LoadFromFirestore<T>(uid, className, "primary");
        }

    }
}