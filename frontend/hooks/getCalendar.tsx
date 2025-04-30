import AsyncStorage from "@react-native-async-storage/async-storage";

export const getCalendarFromStorage = async () => {
    try {
        const jsonValue = await AsyncStorage.getItem("calendar");
        return jsonValue != null ? JSON.parse(jsonValue) : null;
      } catch (e) {
        console.error("Reading error:", e);
        return null;
      }
}