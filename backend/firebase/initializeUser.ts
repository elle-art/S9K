import { getAuth, signInAnonymously } from "firebase/auth";
import { getFirestore, doc, getDoc } from "firebase/firestore";

const auth = getAuth();
const db = getFirestore();


signInAnonymously(auth)
  .then((result) => {
    // user is signed in
    console.log("User is signed in")

    //check if user is new or not
    const user = result.user; // 🔑 this is the signed-in user
    const uid = user.uid;

    // Step 1: Create a reference to the user's document
    const userRef = doc(db, "users", uid);

    // Step 2: Return the getDoc Promise
    return getDoc(userRef);
  })
  .then((userSnap) => {
    if(userSnap.exists()) {
      console.log("Existing user found");
    } else {
      console.log("New user - no user data found");
    }
    
  })
  .catch((error) => {
    const errorCode = error.code;
    const errorMessage = error.message;
    console.log("Error: " + errorCode + ": " + errorMessage);
  });
