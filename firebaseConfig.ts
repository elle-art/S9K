// configures firebase and exports necessary objects


// firebaseConfig.ts
import { initializeApp, getApps, getApp } from "firebase/app";
import { getAuth } from "firebase/auth";
import { getFirestore } from "firebase/firestore";
// Import the functions you need from the SDKs you need
// import { getAnalytics } from "firebase/analytics";
// TODO: Add SDKs for Firebase products that you want to use
// https://firebase.google.com/docs/web/setup#available-libraries

// Your web app's Firebase configuration
// For Firebase JS SDK v7.20.0 and later, measurementId is optional
const firebaseConfig = {
  apiKey: "AIzaSyAY1uADb_PA8pAwCYlbo6t6V3JzWTiS-yo",
  authDomain: "the-scheduler-9000.firebaseapp.com",
  projectId: "the-scheduler-9000",
  storageBucket: "the-scheduler-9000.firebasestorage.app",
  messagingSenderId: "1020443528484",
  appId: "1:1020443528484:web:8c9163fd7241441258b7b1",
  measurementId: "G-CRJDWELBRS"
};

const app = getApps().length === 0 ? initializeApp(firebaseConfig) : getApp();
const auth = getAuth(app);
const db = getFirestore(app);

export { app, auth, db }; // export all three