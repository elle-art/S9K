import { signInAnonymously } from "firebase/auth";
import { doc, getDoc, setDoc } from "firebase/firestore";
import { saveUid, getStoredUid } from "./storageHelper";
import { auth, db } from "@/firebaseConfig"; // use the initialized instances

export const initializeUser = async () => {
  const storedUid = await getStoredUid();

  if (storedUid) {
    const userRef = doc(db, "users", storedUid);
    const userSnap = await getDoc(userRef);

    if (userSnap.exists()) {
      console.log("Returning user. Using stored UID:", storedUid);
      return storedUid;
    } else {
      console.log("Stored UID has no matching Firestore doc. Signing in again...");
    }
  }

  const result = await signInAnonymously(auth);
  const uid = result.user.uid;
  console.log("Signed in anonymously with UID:", uid);

  const userRef = doc(db, "users", uid);
  const userSnap = await getDoc(userRef);

  if (!userSnap.exists()) {
    await setDoc(userRef, {
      createdAt: Date.now(),
    });
    console.log("New account ");
  }

  await saveUid(uid);
  return uid;
};