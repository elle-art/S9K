import React from 'react';
import { View, Text, Button, Alert, StyleSheet } from 'react-native';
import RNCalendarEvents from 'react-native-calendar-events';
import RNFS from 'react-native-fs';

const CalendarLogger = () => {
  const requestAndLogEvents = async () => {
    try {
      const status = await RNCalendarEvents.requestPermissions();

      if (status !== 'authorized') {
        Alert.alert('Permission denied', 'Cannot access calendar events');
        return;
      }

      // Get today's date and one year later
      const today = new Date();
      const nextYear = new Date();
      nextYear.setFullYear(today.getFullYear() + 1);

      // Format them to ISO strings
      const startDate = today.toISOString();
      const endDate = nextYear.toISOString();

      // Fetch calendar events
      const events = await RNCalendarEvents.fetchAllEvents(startDate, endDate);
      console.log(`Fetched ${events.length} events.`);

      // Write to file
      const path = RNFS.DocumentDirectoryPath + '/calendar_log.txt';
      await RNFS.writeFile(path, JSON.stringify(events, null, 2), 'utf8');

      Alert.alert('Success', `Events logged to:\n${path}`);
    } catch (error) {
      console.error('Error:', error);
      Alert.alert('Error', 'Something went wrong.');
    }
  };

  return (
    <View style={styles.container}>
      <Text style={styles.title}>Calendar Logger</Text>
      <Button title="Log Calendar Events" onPress={requestAndLogEvents} />
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: '#fff',
    padding: 16,
  },
  title: {
    fontSize: 24,
    marginBottom: 20,
  },
});

export default CalendarLogger;
