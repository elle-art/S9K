using Google.Cloud.Firestore;

namespace Backend.Models {

[FirestoreData]
public class Dummy {

    [FirestoreProperty]
    public string Name {get; set;}
    [FirestoreProperty]
    public int Number {get; set;}

    public Dummy(string name, int number) {
        Name = name;
        Number = number;
    }

    // required for firestore
    public Dummy() {}


}
}