// User Interface for UserProvider
'use client'
import { createContext, ReactNode, useState, useEffect, useContext } from "react";
import { S9KUser } from "@/frontend/constants/User";
import { getUserFromStorage } from "@/frontend/hooks/getUser";

interface UserContextProps {
  user: S9KUser | null;
  setUser: (user: S9KUser | null) => void;
}

const UserContext = createContext<UserContextProps> ({
  user: null,
  setUser: () => {},
});

interface UserProviderProps {
  children: ReactNode;
}

export const UserProvider = ({ children }: UserProviderProps) => {
  const [user, setUser] = useState<S9KUser | null>(null);

  useEffect(() => {
    (async () => {
        const storedUser = await getUserFromStorage();
        if (storedUser && storedUser !== user) {
          setUser(storedUser);
        }
      })();
  }, []);

  return (
    <UserContext.Provider value={{ user, setUser }}>
      {children}
    </UserContext.Provider>
  );
};

export const useUser = () => useContext(UserContext);
