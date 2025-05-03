import { Tabs } from 'expo-router';
import React, { useEffect, useState } from 'react';
import { Platform, StyleSheet, TextInput, Button } from 'react-native';
import { HapticTab } from '@/components/HapticTab';
import TabBarBackground from '@/components/ui/TabBarBackground';
import { Colors } from '@/frontend/constants/Colors';
import { useColorScheme } from '@/frontend/hooks/useColorScheme';
import FontAwesome5 from '@expo/vector-icons/FontAwesome5';
import SimpleLineIcons from '@expo/vector-icons/SimpleLineIcons';
import { ThemedText } from '@/components/ThemedText';
import { ThemedView } from '@/components/ThemedView';
import { hasUsername as checkUsernameExists } from '@/frontend/firebase-api/userHelper';

export default function TabLayout() {
  const colorScheme = useColorScheme();
  const [hasUsername, setHasUsername] = useState<boolean | null>(null); // null = loading
  const [name, onChangeName] = useState(''); 

  useEffect(() => {
    const checkUser = async () => {
      const result = await checkUsernameExists();
      setHasUsername(result);
    };

    checkUser();
  }, []);

  if (hasUsername === null) {
    // Optional: add a loading spinner here
    return null;
  }

  if (!hasUsername) {
    return (
      <ThemedView style={{ width: "100%", height: "100%" }}>
        <ThemedView style={styles.titleContainer}>
          <ThemedText type="title" >Welcome to</ThemedText>
          <ThemedText type="title">The Scheduler 9000!</ThemedText>
        </ThemedView>

        <ThemedView style={styles.inputContainer}>
          <ThemedText type="subtitle">Enter your display name:</ThemedText>
          <TextInput
            style={styles.input}
            onChangeText={onChangeName}
            value={name}
            placeholder="@bobbyjoe123"
            keyboardType="ascii-capable"
          />
        </ThemedView>
         <Button
          title="Create Account"
          onPress={async () => {
            if (name) {
              const result = await checkUsernameExists(); // re-check username state
              setHasUsername(result);
            } else {
              alert('Please enter a valid name');
            }
          }}
        />
      </ThemedView>
    );
  } else {
    return (
      <Tabs
        screenOptions={{
          tabBarActiveTintColor: Colors[colorScheme ?? 'light'].tint,
          headerShown: false,
          tabBarButton: HapticTab,
          tabBarBackground: TabBarBackground,
          tabBarStyle: Platform.select({
            ios: {
              position: 'absolute',
            },
            default: {},
          }),
        }}>
        <Tabs.Screen
          name="index"
          options={{
            title: 'Calendar',
            tabBarIcon: ({ color }) => <FontAwesome5 name="calendar-alt" size={24} color={color} />,
          }}
        />
        <Tabs.Screen
          name="taskList"
          options={{
            title: 'Tasks',
            tabBarIcon: ({ color }) => <FontAwesome5 name="list-alt" size={24} color={color} />,
          }}
        />
        <Tabs.Screen
          name="menu"
          options={{
            title: 'Menu',
            tabBarIcon: ({ color }) => <SimpleLineIcons name="menu" size={24} color={color} />,
          }}
        />
      </Tabs>
    );
  }
}

const styles = StyleSheet.create({
  titleContainer: {
    flexDirection: 'column',
    alignItems: 'center',
    gap: 8,
    marginTop: 180,
  },
  inputContainer: {
    gap: 8,
    marginTop: 100,
    marginBottom: 8,
    marginLeft: 25,
    marginRight: 25,
  },
  input: {
    height: 40,
    margin: 12,
    borderWidth: 1,
    borderRadius: 5,
    padding: 10,
  },
});
