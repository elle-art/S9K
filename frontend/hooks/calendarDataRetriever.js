import * as Calendar from 'expo-calendar';
import * as FileSystem from 'expo-file-system';

export const retrieveAndStoreCalendarEvents = async () => {
  try {
    console.log("Running calendarDataRetriever");
    const { status } = await Calendar.requestCalendarPermissionsAsync();
    if (status !== 'granted') {
      console.error('Permission denied: Cannot access calendar events');
      return;
    }

    const calendars = await Calendar.getCalendarsAsync(Calendar.EntityTypes.EVENT);
    const calendarIds = calendars.map(c => c.id);

    const today = new Date();
    const nextYear = new Date();
    nextYear.setFullYear(today.getFullYear() + 1);

    let allEvents = [];

    for (const id of calendarIds) {
      const events = await Calendar.getEventsAsync([id], today, nextYear);
      allEvents.push(...events);
    }

    const filePath = FileSystem.documentDirectory + 'events.json';
    await FileSystem.writeAsStringAsync(filePath, JSON.stringify(allEvents, null, 2));
    console.log('Events logged to:', filePath);
  } catch (error) {
    console.error('Error retrieving calendar events:', error);
  }
};
