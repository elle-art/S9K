using Backend.Models;

namespace Backend.Services
{
    public class DBCommunications
    {

        public static async Task SaveDummyAsync(string uid, Dummy dummy)
        {
            await FireBaseCommunications.SaveToFirestore(uid, dummy);
        }

        public static async Task<Dummy> GetDummyAsync(string uid)
        {
            return await FireBaseCommunications.LoadFromFirestore(uid);
        }
    }
}