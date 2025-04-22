using Backend.Models;
using Google.Cloud.Firestore;
using System.Threading.Tasks;

namespace Backend.Services {
public static class FirebaseCommunications
{
    private static readonly FirestoreDb db = FirestoreDb.Create("the-scheduler-9000");

    public static async Task SaveToFirestore(string uid, Dummy dummy)
    {
        DocumentReference docRef = db
            .Collection("users")
            .Document(uid)
            .Collection("dummy")
            .Document("primary");

        await docRef.SetAsync(dummy);
    }


    public static async Task<Dummy> LoadFromFirestore(string uid)
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
}
}