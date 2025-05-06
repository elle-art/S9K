
// ─────────────────────────
#if UNIT_TEST
#define USE_INMEM_DB 
#endif
// ─────────────────────────────────────────────────────────────────────────────
using Backend.Models;

namespace Backend.Services
{
    public class DBCommunications
    {
#if USE_INMEM_DB   // ───────────── 2.  in‑memory stub for tests  ──────────────
    private static readonly Dictionary<string, object> _mem =
        new(StringComparer.OrdinalIgnoreCase);

    // 🔒 1.  private helper that really writes to the dictionary
    private static Task SaveByKeyAsync<T>(string key, T obj)
    {
        _mem[key] = obj!;
        return Task.CompletedTask;
    }

    // 2️⃣  public overload used by most code  (uid + className + obj)
    public static Task SaveObjectAsync<T>(string uid, string className, T obj)
        => SaveByKeyAsync($"{uid}:{className.ToLower()}", obj);

    // 3️⃣  convenience overload  (uid + obj)
    public static Task SaveObjectAsync<T>(string uid, T obj)
        => SaveObjectAsync(uid, typeof(T).Name.ToLower(), obj);

    // 4️⃣  read helper  (uid + className)
    public static Task<T?> GetObjectAsync<T>(string uid, string className)
    {
        _mem.TryGetValue($"{uid}:{className.ToLower()}", out var val);
        return Task.FromResult((T?)val);
    }
#else               // ─────────── 3.  real firebase code  ────────────────

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

    #endif
    }
    
}