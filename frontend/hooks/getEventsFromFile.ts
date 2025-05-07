import * as FileSystem from 'expo-file-system';

const FILE_URI = `${FileSystem.documentDirectory}events.json`;

export const getEventsFromPhoneCalendar = async () => {
  try {
    const fileInfo = await FileSystem.getInfoAsync(FILE_URI);
    console.log("fileInfo didn't silently fail");
    if (!fileInfo.exists) {
      console.log('File does not exist. Creating a new one.');
      await FileSystem.writeAsStringAsync(FILE_URI, JSON.stringify([])); // write empty array
    }

    const fileContents = await FileSystem.readAsStringAsync(FILE_URI);
    const parsedEvents = JSON.parse(fileContents);

    return parsedEvents;
  } catch (error) {
    //console.log('Caught error reading file:', error.message);
    console.log('Caught error object:', error);
    return [];
  }
};
