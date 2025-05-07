import { signInAnonymously } from "firebase/auth";
import { doc, getDoc, setDoc } from "firebase/firestore";
import { saveUid, getStoredUid } from "./storageHelper";
import { auth, db } from "@/firebaseConfig"; // use the initialized instances
import { Calendar } from "../constants/Calendar";
import { S9KUser } from "../constants/User";
import AsyncStorage from "@react-native-async-storage/async-storage";
import { useUser } from "../utils/user/userProvider";

export const initializeUser = async (name?: string) => {
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

export const createUserInfo = async (displayName: string) => {
  const storedUid = await getStoredUid();

  try {
    // Reference to the 'primary' document in the 'userinfo' subcollection
    const userInfoRef = doc(db, 'users', storedUid!, 'userinfo', 'primary');

    // Save additional user info to Firestore
    await setDoc(userInfoRef, {
      displayName,
      createdAt: Date.now(),
    });

    // Store user data in AsyncStorage
    const userData = {
      displayName: displayName,
      invites: [],
      tasks: [],
      weeklyGoal: ''
    };
    const calendarData = {
      availability: [],
      preferredTimes: [],
      events: []
    }
    await AsyncStorage.setItem('user', JSON.stringify(userData));
    await AsyncStorage.setItem('calendar', JSON.stringify(calendarData));


    const { setUser } = useUser();
    setUser(userData);

    console.log('User account created & signed in!');
  } catch (error) {
    console.error('Error creating account:', error);
  }
};

export function buildFirebaseDataDoc(user: S9KUser, calendar: Calendar): any {
  console.log(user)
  console.log(calendar);
  return {
    displayName: user.displayName,
    inviteInbox: user.invites,
    taskList: user.tasks,
    weeklyGoal: user.weeklyGoal,
    preferredTimes: calendar.preferredTimes ?? [],
    userAvailability: calendar.availability,
    userCalendar: calendar.events,
  };
}
