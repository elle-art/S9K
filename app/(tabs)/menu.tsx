import { Image, StyleSheet, TouchableOpacity } from 'react-native';
import React, { useEffect, useState } from 'react';
import FontAwesome from '@expo/vector-icons/FontAwesome';
import { HelloWave } from '@/components/menu/HelloWave';
import ParallaxScrollView from '@/components/ParallaxScrollView';
import { ThemedText } from '@/components/ThemedText';
import { ThemedView } from '@/components/ThemedView';
import { useUser } from '@/frontend/utils/user/userProvider';
import { getCalendarFromStorage } from '@/frontend/hooks/getCalendar';
import { Event } from '@/frontend/constants/Calendar';
import { EventCard } from '@/components/EventCard';
import { useRouter } from 'expo-router';
// import { AvailabilityGraph } from '@/components/menu/AvailabilityGraph';

// TO-DO: create indicator for new inbox message
// TO-DO:  create button components/onPress()

export default function ProfileScreen() {
  const [calendar, setCalendar] = useState<any>(null);
  const { user } = useUser();
  const router = useRouter();

  useEffect(() => {
    (async () => {
      const data = await getCalendarFromStorage();
      setCalendar(data);
    })();
  }, []);

  return (
    <ParallaxScrollView
      headerBackgroundColor={{ light: '#A1CEDC', dark: '#1D3D47' }}
      headerImage={
        <Image
          source={require('@/frontend/assets/images/partial-react-logo.png')}
          style={styles.reactLogo}
        />
      }>
      <ThemedView style={styles.titleContainer}>
        <ThemedText type="title">@{user?.displayName}</ThemedText>
        <HelloWave />
        <TouchableOpacity style={styles.settingsButton} onPress={() => router.push('/menu/settings')}>
          <FontAwesome name="cog" size={24} color={'#fff'} />
        </TouchableOpacity>
      </ThemedView>
      <ThemedView style={styles.stepContainer}>
        <ThemedText type="default">"I, {user?.displayName}, will {user?.weeklyGoal ?? 'schedule more!'}"</ThemedText>
      </ThemedView>

      <ThemedView style={styles.titleContainer}>
        <ThemedText type="subtitle">Availability</ThemedText>
        <TouchableOpacity
          onPress={() => {
            // Your handler here
            console.log('Edit button pressed');
          }}
          style={{
            position: 'absolute',
            right: -10,
            paddingVertical: 2,
            paddingHorizontal: 10,
            backgroundColor: '#1D3D47',
            borderRadius: 5,
          }}
        >
          <ThemedText>Edit</ThemedText>
        </TouchableOpacity>
      </ThemedView>
      <ThemedView style={styles.stepContainer}>
        {/* <AvailabilityGraph /> */}
      </ThemedView>

      <ThemedView style={styles.stepContainer}>
        <ThemedView style={styles.titleContainer}>
          <ThemedText type="subtitle">Events</ThemedText>
          <TouchableOpacity
            onPress={() => {
              // Your handler here
              console.log('Edit button pressed');
            }}
            style={{
              position: 'absolute',
              right: -10,
              paddingVertical: 2,
              paddingHorizontal: 10,
              backgroundColor: '#1D3D47',
              borderRadius: 5,
            }}
          >
            <ThemedText>View Inbox</ThemedText>
          </TouchableOpacity>
        </ThemedView>

        <ThemedText type="defaultSemiBold">Upcoming</ThemedText>
        {calendar?.events
          ?.filter((event: Event) => new Date(event.date) >= new Date())
          .map((event: Event, index: number) => (
            <ThemedView key={index} style={{width: 350}}>
              <EventCard event={event} />
            </ThemedView>
          ))}
        <ThemedText type="defaultSemiBold">Previous</ThemedText>
        {calendar?.events
          ?.filter((event: Event) => new Date(event.date) < new Date())
          .map((event: Event, index: number) => (
            <ThemedView key={index} style={{width: 350}}>
              <EventCard event={event} />
            </ThemedView>
          ))}
      </ThemedView>
    </ParallaxScrollView>
  );
}

const styles = StyleSheet.create({
  titleContainer: {
    flexDirection: 'row',
    alignItems: 'center',
    gap: 8,
  },
  stepContainer: {
    gap: 8,
    marginBottom: 8,
  },
  reactLogo: {
    height: 178,
    width: 290,
    bottom: 0,
    left: 0,
    position: 'absolute',
  },
  settingsButton: {
    position: 'absolute',
    right: -10,
    top: -3,
    paddingVertical: 4,
    paddingHorizontal: 10,
    backgroundColor: '#1D3D47',
    borderRadius: 5
  }
});
