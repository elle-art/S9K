import { getFirestore, doc, getDoc } from "firebase/firestore";
import { getAuth } from "firebase/auth";

export async function hasUsername(): Promise<boolean> {
  const auth = getAuth();
  const user = auth.currentUser;

  if (!user) return false;

  const db = getFirestore();
  const userRef = doc(db, "users", user.uid);
  const snap = await getDoc(userRef);

  return snap.exists() && !!snap.data()?.displayName;
}
