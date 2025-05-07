import AsyncStorage from '@react-native-async-storage/async-storage';
import { User } from '../constants/User';
import { collection, getDocs, query, where } from "firebase/firestore";
import { db } from "@/firebaseConfig";

//TO-DO: Pull the data stored in firebaseDataDoc from firebase
// export const firebaseDataDoc = {
//     displayName: "johnny123",
//     inviteInbox: [
//         {
//             fromUser: "jane_abc",
//             message: "Join the planning session!",
//             event: {
//                 name: "Planning Session",
//                 date: "2025-04-27",
//                 time: { startTime: "14:00", endTime: "15:00" },
//                 type: "group",
//                 group: []
//             }
//         }
//     ],
//     taskList: [
//         { name: "Submit report", status: false, date: "2025-04-26" }
//     ],
//     weeklyGoal: "complete 3 tasks by the end of the week",
//     preferredTimes: [],
//     userAvailability: [
//         { "day": 0, "startTime": "09:00", "endTime": "12:00" },
//         { "day": 2, "startTime": "14:00", "endTime": "17:00" }
//     ],
//     userCalendar: [
//         {
//             name: "Team Check-in",
//             date: "2025-04-30",
//             time: { startTime: "10:00", endTime: "10:30" },
//             group: []
//         },
//         {
//             name: "Team Check-in",
//             date: "2025-05-02",
//             time: { startTime: "10:00", endTime: "10:30" },
//             group: [],
//             type: 'productive'
//         },
//         {
//             name: "Team Check-in",
//             date: "2025-05-2",
//             time: { startTime: "14:00", endTime: "16:30" },
//             group: [],
//             type: 'social'
//         },
//         {
//             name: "Team Check-in",
//             date: "2025-05-2",
//             time: { startTime: "1:00", endTime: "1:30" },
//             group: [],
//             type: 'personal'
//         }
//     ]
// };

export async function saveUserData(uid: string, user: User) {

    try {
        // Send a POST request to your backend controller's save endpoint
        const response = await fetch(`http://10.255.5.83:5000/api/user/save?uid=${uid}`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(uid), // Send the data to the backend
        });

        const result = await response.json();
        console.log('Response:', result);
        if (response.ok) {
            console.log("Data successfully saved:", result);
        } else {
            console.error("Failed to save data:", result);
        }
    } catch (error) {
        console.error("Error while saving data:", error);
    }
}

// TO-DO: add real firebase data
export async function saveDataToStorage(firebaseDataDoc: any) {
    const user = {
        displayName: firebaseDataDoc.displayName,
        invites: firebaseDataDoc.inviteInbox,
        tasks: firebaseDataDoc.taskList,
        weeklyGoal: firebaseDataDoc.weeklyGoal
    };

    const calendar = {
        availability: firebaseDataDoc.userAvailability,
        events: firebaseDataDoc.userCalendar
    };

    try {
        const userJson = JSON.stringify(user);
        await AsyncStorage.setItem("user", userJson);

        const calJson = JSON.stringify(calendar);
        await AsyncStorage.setItem("calendar", calJson);

        console.log("user", userJson);
        console.log("calendar", calJson);
    } catch (e) {
        console.error("Saving error:", e);
    }
};

export async function getUserByDisplayName(displayName: string) {
    const usersRef = collection(db, "users");
    const q = query(usersRef, where("displayName", "==", displayName));
    const querySnapshot = await getDocs(q);
  
    if (!querySnapshot.empty) {
      const userDoc = querySnapshot.docs[0];
      console.log("User found:", userDoc.id, userDoc.data());
      return userDoc.data();
    } else {
      console.log("No user found with displayName:", displayName);
      return null;
    }
}
