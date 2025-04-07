import { Tabs } from 'expo-router';
import React from 'react';
import { Platform, StyleSheet, SafeAreaView, TextInput } from 'react-native';
import { HapticTab } from '@/components/HapticTab';
import TabBarBackground from '@/components/ui/TabBarBackground';
import { Colors } from '@/frontend/constants/Colors';
import { useColorScheme } from '@/frontend/hooks/useColorScheme';
import FontAwesome5 from '@expo/vector-icons/FontAwesome5';
import SimpleLineIcons from '@expo/vector-icons/SimpleLineIcons';
import { ThemedText } from '@/components/ThemedText';
import { ThemedView } from '@/components/ThemedView';
import { SafeAreaProvider } from 'react-native-safe-area-context';

export default function TabLayout() {
  const colorScheme = useColorScheme();

  const user = false; // ****add logic to see if user account exists

  const [name, onChangeName] = React.useState(''); // ***add logic to create user account after input in onChangeName function

  if (user) {
    return (                                                                                      <SafeAreaProvider style={{ backgroundColor: "#fff" }}>
        <ThemedView style={{ maxWidth: "80%", height: "100%", marginLeft: "10%", marginRight: "10%" }}>
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
        </ThemedView>
      </SafeAreaProvider>
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
              // Use a transparent background on iOS to show the blur effect
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
    marginTop: 260,
  },
  inputContainer: {
    gap: 8,
    marginTop: 100,
    marginBottom: 8,
  },
  input: {
    height: 40,
    margin: 12,
    borderWidth: 1,
    borderRadius: 5,
    padding: 10,
  },
});
