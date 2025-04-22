using Backend.Models;

namespace Backend.Services
{
    public class DBCommunications
    {

        public static async Task SaveDummyAsync(string uid, Dummy dummy)
        {
            await FirebaseCommunications.SaveToFirestore(uid, dummy);
        }

        public static async Task<Dummy> GetDummyAsync(string uid)
        {
            return await FirebaseCommunications.LoadFromFirestore(uid);
        }
    }
}