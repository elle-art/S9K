import { Image, StyleSheet, TouchableOpacity } from 'react-native';
import React, { useState } from 'react';
import FontAwesome from '@expo/vector-icons/FontAwesome';
import { HelloWave } from '@/components/menu/HelloWave';
import ParallaxScrollView from '@/components/ParallaxScrollView';
import { ThemedText } from '@/components/ThemedText';
import { ThemedView } from '@/components/ThemedView';
import { useUser } from '@/frontend/utils/user/userProvider';
import { router } from 'expo-router';

export default function ProfileScreen() {
  const [message, setMessage] = useState('Loading...');
  const { user } = useUser();

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
        <TouchableOpacity
          style={styles.iconButton}
          onPress={() => router.push('/settings')}
        >
          <FontAwesome name="cog" size={24} color={'#fff'} />
        </TouchableOpacity>
      </ThemedView>

      <ThemedView style={styles.stepContainer}>
        <ThemedText type="default">
          "I, {user?.displayName}, will {user?.weeklyGoal}"
        </ThemedText>
      </ThemedView>

      <ThemedView style={styles.titleContainer}>
        <ThemedText type="subtitle">Availability</ThemedText>
        <TouchableOpacity
          onPress={() => router.push('/editAvailability')}
          style={styles.textButton}
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
            onPress={() => router.push('/inbox')}
            style={styles.textButton}
          >
            <ThemedText>View Inbox</ThemedText>
          </TouchableOpacity>
        </ThemedView>

        <ThemedText type="defaultSemiBold">Upcoming</ThemedText>
        <ThemedText type="defaultSemiBold">Previous</ThemedText>
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
  iconButton: {
    position: 'absolute',
    right: -10,
    top: -3,
    paddingVertical: 4,
    paddingHorizontal: 10,
    backgroundColor: 'blue',
    borderRadius: 5,
  },
  textButton: {
    position: 'absolute',
    right: -10,
    paddingVertical: 2,
    paddingHorizontal: 10,
    backgroundColor: 'blue',
    borderRadius: 5,
  },
});
