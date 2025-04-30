import { useThemeColor } from "@/frontend/hooks/useThemeColor";
import { Dimensions, View } from "react-native";
import { BarChart } from "react-native-gifted-charts";
import { ThemedTextProps } from "../ThemedText";

type AvailabilityGraphProps = {
  color: string;
};


export function AvailabilityGraph ({
  style,
  lightColor,
  darkColor,
  type = 'default',
  ...rest
}: ThemedTextProps) {
  const color = useThemeColor({ light: darkColor, dark:  lightColor  }, 'text');

    //TO DO: Dynamically generate stack based on availability and preferred times
    const stackData = [
        {
          stacks: [
            {value: 10, color: 'orange'},
            {value: 20, color: '#4ABFF4', marginBottom: 2},
          ],
          label: 'M',
        },
        {
          stacks: [
            {value: 10, color: '#4ABFF4'},
            {value: 11, color: 'orange', marginBottom: 2},
            {value: 15, color: '#28B2B3', marginBottom: 2},
          ],
          label: 'T',
        },
        {
          stacks: [
            {value: 14, color: 'orange'},
            {value: 18, color: '#4ABFF4', marginBottom: 2},
          ],
          label: 'W',
        },
        {
          stacks: [
            {value: 7, color: '#4ABFF4'},
            {value: 11, color: 'orange', marginBottom: 2},
            {value: 10, color: 'rgba(0,0,0,0)', marginBottom: 2},
          ],
          label: 'T',
        },
        {
            stacks: [
              {value: 7, color: '#4ABFF4'},
              {value: 11, color: 'orange', marginBottom: 2},
              {value: 10, color: 'rgba(0,0,0,0)', marginBottom: 2},
            ],
            label: 'F',
          },
          {
            stacks: [
              {value: 7, color: '#4ABFF4'},
              {value: 11, color: 'orange', marginBottom: 2},
              {value: 10, color: 'rgba(0,0,0,0)', marginBottom: 2},
            ],
            label: 'S',
          },
          {
            stacks: [
              {value: 7, color: '#transparent'},
              {value: 11, color: 'orange', marginBottom: 2},
              {value: 10, color: 'rgba(0,0,0,0)', marginBottom: 2},
            ],
            label: 'S',
          },
      ];
    return(
        <View>
            <BarChart
            width={260}
            barWidth={12}
            spacing={24}
            noOfSections={7}
            barBorderRadius={2}
            xAxisColor={color}
            yAxisColor={color}
            yAxisTextStyle={color}
            xAxisLabelTextStyle={color}
            stackData={stackData}
            />
        </View>
    );
};
