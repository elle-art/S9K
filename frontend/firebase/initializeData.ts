import AsyncStorage from '@react-native-async-storage/async-storage';

//TO-DO: Pull the data stored in firebaseDataDoc from firebase
export const firebaseDataDoc = {
    displayName: "johnny123",
    invites: [
        {
            fromUser: "jane_abc",
            message: "Join the planning session!",
            event: {
                name: "Planning Session",
                date: "2025-04-27",
                time: { startTime: "14:00", endTime: "15:00" },
                type: "group",
                group: ["johnny123", "jane_abc"]
            }
        }
    ],
    tasks: [
        { name: "Submit report", status: false, date: "2025-04-26" }
    ],
    weeklyGoal: "complete 3 tasks by the end of the week",
    availability: [
        { startTime: "09:00", endTime: "12:00" },
        { startTime: "14:00", endTime: "17:00" }
    ],
    events: [
        {
            name: "Team Check-in",
            date: "2025-04-30",
            time: { startTime: "10:00", endTime: "10:30" },
            group: ["johnny123", "joe456"]
        },
        {
            name: "Team Check-in",
            date: "2025-05-02",
            time: { startTime: "10:00", endTime: "10:30" },
            group: ["johnny123", "joe456"],
            type: 'productive'
        },
        {
            name: "Team Check-in",
            date: "2025-05-2",
            time: { startTime: "14:00", endTime: "16:30" },
            group: ["johnny123", "joe456"],
            type: 'social'
        },
        {
            name: "Team Check-in",
            date: "2025-05-2",
            time: { startTime: "1:00", endTime: "1:30" },
            group: ["johnny123", "joe456"],
            type: 'personal'
        }
    ]
};

export async function saveDataToStorage(firebaseDataDoc: any) {
    const user = {
        displayName: firebaseDataDoc.displayName,
        invites: firebaseDataDoc.invites,
        tasks: firebaseDataDoc.tasks,
        weeklyGoal: firebaseDataDoc.weeklyGoal
    };

    const calendar = {
        availability: firebaseDataDoc.availability,
        events: firebaseDataDoc.events
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
