import React, { useState } from 'react';
import { View, TextInput, Button, StyleSheet, Text, FlatList, Alert } from 'react-native';
import { ThemedView } from '@/components/ThemedView';
import { ThemedText } from '@/components/ThemedText';
import axios from 'axios';
import DateTimePicker from '@react-native-community/datetimepicker';
import RNCalendarEvents from 'react-native-calendar-events'; // import the package


export default function InviteScreen() {
  const [search, setSearch] = useState('');
  const [foundUsers, setFoundUsers] = useState<string[]>([]);
  const [foundUserIDs, setFoundUserIDs] = useState<string[]>([]);
  const [errorMessage, setErrorMessage] = useState('');
  const [eventName, setEventName] = useState('');
  const [eventDescription, setEventDescription] = useState('');
  const [eventDate, setEventDate] = useState(new Date());
  const [showDatePicker, setShowDatePicker] = useState(false);
  const [eventDuration, setEventDuration] = useState('');
  const [generatedTime, setGeneratedTime] = useState<Date | null>(null);

  <InviteScreen addEventToCalendar={this.addEvent} />

  const allFieldsFilled = (): boolean => {
    return (
      eventName.trim() &&
      eventDescription.trim() &&
      eventDuration.trim() &&
      foundUserIDs.length > 0
    );
  };

  const generateTime = async () => {
    if (!allFieldsFilled()) {
      Alert.alert('Missing Information', 'Please fill out all fields and search for at least one user.');
      return;
    }

    try {
      const payload = {
        invitedUserIds: foundUserIDs,
        eventName,
        eventDescription,
        durationMinutes: parseInt(eventDuration),
        date: eventDate.toISOString(),
      };

      const response = await axios.post(
        'http://172.16.2.249:5000/api/user/generate-event-time',
        payload
      );

      const [suggestedDateTime] = response.data;
      setGeneratedTime(new Date(suggestedDateTime));
    } catch (error) {
      console.error(error);
      Alert.alert('Error', 'Failed to generate event time.');
    }
  };

  const sendInvite = () => {
    Alert.alert('Invite Sent', `Event scheduled at ${generatedTime?.toLocaleString()}`);
    addEventToCalendar(generatedTime, eventName, eventDescription);
  };

  const onChangeDate = (event: any, selectedDate?: Date) => {
    setShowDatePicker(false);
    if (selectedDate) {
      setEventDate(selectedDate);
    }
  };

  const searchUser = async (displayName: string) => {
    if (!displayName.trim()) {
      setErrorMessage('Please enter a display name');
      return;
    }

    try {
      const response = await axios.get(
        `http://172.16.2.249:5000/api/user/check-username?username=${displayName}`

      );

      if (response.data.exists) {
        setFoundUsers((prev) => [...prev, displayName]);
        setFoundUserIDs((prev) => [...prev, response.data.userId]);
        setErrorMessage('');
        const userId = response.data.userId;
          console.log("(ID: " + userId + ")");
      } else {
        setErrorMessage(`User '${displayName}' not found`);
      }
    } catch (error) {
      setErrorMessage('Error searching for user');
      console.error(error);
    }
  };

  const clearResults = () => {
    setFoundUsers([]);
    setFoundUserIDs([]);
    setErrorMessage('');
  };

  return (
    <ThemedView style={styles.container}>
      <ThemedText type="title" style={styles.title}>User Search</ThemedText>

      <TextInput
        style={styles.searchInput}
        placeholder="Search by display name..."
        value={search}
        onChangeText={setSearch}
      />

      <View style={styles.buttonContainer}>
        <Button title="Search" onPress={() => searchUser(search)} />
        <View style={styles.buttonSpacer} />
        <Button title="Clear" color="gray" onPress={clearResults} />
      </View>

      {errorMessage !== '' && (
        <Text style={styles.errorText}>{errorMessage}</Text>
      )}

      {foundUsers.length > 0 && (
        <View style={styles.resultsContainer}>
          <Text style={styles.resultsTitle}>Found Users:</Text>
          <FlatList
            data={foundUsers}
            keyExtractor={(item, index) => `${item}-${index}`}
            renderItem={({ item, index }) => (
              <Text style={styles.userItem}>
                {item} (ID: {foundUserIDs[index]})

              </Text>
            )}
          />
        </View>
      )}

      <View style={styles.eventInputContainer}>
        <Text style={styles.sectionTitle}>Create Event</Text>

        <TextInput
          style={styles.input}
          placeholder="Event Name"
          value={eventName}
          onChangeText={setEventName}
          placeholderTextColor="#666"
        />
        <View style={styles.eventTypeContainer}>
          <Text style={styles.eventTypeLabel}>Event type:</Text>
          <TextInput
            style={[styles.input, styles.eventTypeInput]}
            placeholder="e.g. Meeting"
            value={eventDescription}
            onChangeText={setEventDescription}
            placeholderTextColor="#666"
          />
        </View>

        <TextInput
          style={styles.input}
          placeholder="Duration (minutes)"
          value={eventDuration}
          onChangeText={setEventDuration}
          keyboardType="numeric"
          placeholderTextColor="#666"
        />
      </View>

      <View>
        <Text style={styles.sectionTitle}>Select Date</Text>
        <Button
          title={eventDate.toDateString()}
          onPress={() => setShowDatePicker(true)}
          color="#4CAF50"
        />
        {showDatePicker && (
          <DateTimePicker
            value={eventDate}
            mode="date"
            display="default"
            onChange={onChangeDate}
          />
        )}
      </View>

      <View style={{ marginTop: 24 }}>
        <Button
          title="Generate Time"
          onPress={generateTime}
          disabled={!allFieldsFilled()}
          color="#2196F3"
        />
      </View>

      {generatedTime && (
        <View style={{ marginTop: 16 }}>
          <Button
            title={`Create and Send Invite (${generatedTime.toLocaleString()})`}
            onPress={sendInvite}
            color="#673AB7"
          />
        </View>
      )}
    </ThemedView>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    padding: 16,
    paddingTop: 48,
    color: 'white',
  },
  title: {
    marginBottom: 12,
  },
  searchInput: {
    borderWidth: 1,
    borderRadius: 6,
    padding: 10,
    color: 'white',
    marginBottom: 16,
  },
  buttonContainer: {
    flexDirection: 'row',
    alignItems: 'center',
  },
  buttonSpacer: {
    width: 12,
  },
  errorText: {
    color: 'red',
    marginTop: 12,
  },
  resultsContainer: {
    marginTop: 20,
  },
  resultsTitle: {
    fontWeight: 'bold',
    marginBottom: 8,
    color: 'white',
  },
  userItem: {
    fontSize: 16,
    paddingVertical: 2,
    color: 'white',
  },
  eventInputContainer: {
    marginTop: 32,
  },
  sectionTitle: {
    fontWeight: 'bold',
    fontSize: 16,
    marginBottom: 8,
    color: 'white',
  },
  input: {
    borderWidth: 1,
    borderRadius: 6,
    padding: 10,
    marginBottom: 12,
    color: 'black',
    backgroundColor: 'white',
  },
  eventTypeContainer: {
    flexDirection: 'row',
    alignItems: 'center',
    marginBottom: 12,
  },
  eventTypeLabel: {
    marginRight: 8,
    color: 'white',
    fontWeight: '500',
  },
  eventTypeInput: {
    flex: 1,
    borderWidth: 1,
    borderRadius: 6,
    padding: 10,
    backgroundColor: 'white',
    color: 'black',
  },
});
