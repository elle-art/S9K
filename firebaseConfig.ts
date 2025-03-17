// firebaseConfig.ts
// Import the functions you need from the SDKs you need
import { initializeApp, getApps, getApp } from "firebase/app";
import { getAuth } from "firebase/auth";
import { getFirestore } from "firebase/firestore";

// TODO: Add SDKs for Firebase products that you want to use
// https://firebase.google.com/docs/web/setup#available-libraries

// Your web app's Firebase configuration
const firebaseConfig = {
  apiKey: "AIzaSyBXb94KMpjEORr5u6qfOL7c7Jil0Gjc9vA",
  authDomain: "thes9k.firebaseapp.com",
  projectId: "thes9k",
  storageBucket: "thes9k.firebasestorage.app",
  messagingSenderId: "250242862692",
  appId: "1:250242862692:web:98152e82c52964a5a91971"
};

// Initialize Firebase
const app = getApps().length === 0 ? initializeApp(firebaseConfig) : getApp();

export const auth = getAuth(app);
export const db = getFirestore(app);
export default app;
