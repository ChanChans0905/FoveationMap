import pandas as pd
import numpy as np
import matplotlib.pyplot as plt

# Load the updated data with the 'scenario' column
data_with_scenario = pd.read_csv('C:/Users/dlgoc/Downloads/Spyder/data.txt', sep='\t')

# Determine the number of unique scenarios for color mapping
unique_scenarios = data_with_scenario['scenario'].unique()
num_unique_scenarios = len(unique_scenarios)

# Set the DPI for all plots in this session
plt.rcParams['figure.dpi'] = 500


# Generate a color palette for the scenarios
colors = plt.cm.viridis(np.linspace(0, 1, num_unique_scenarios))

# Initialize the figure for plotting
plt.figure(figsize=(12, 8))

# Find unique 'x' values to create x-ticks labels
unique_x = data_with_scenario['x'].unique()

# Plot each 'x' value and scenario
for x_value in unique_x:
    for scenario in unique_scenarios:
        specific_data = data_with_scenario[(data_with_scenario['x'] == x_value) & (data_with_scenario['scenario'] == scenario)]
        
        if not specific_data.empty:
            x_index = np.where(unique_x == x_value)[0][0]
            scenario_index = np.where(unique_scenarios == scenario)[0][0]
            
            # Calculate the position for each bar
            x_position = x_index + (scenario_index - num_unique_scenarios / 2) * (0.8 / num_unique_scenarios)
            
            # Plot the bar and label for the first occurrence
            plt.bar(x_position, specific_data['y'].values[0], width=0.8/num_unique_scenarios, color=colors[scenario_index], label=f'Scenario {scenario}' if x_value == unique_x[0] else "")
            
            # Label the bar with the 'y' value
            plt.text(x_position, specific_data['y'].values[0] + 0.01, round(specific_data['y'].values[0], 2), ha='center', va='bottom')

# Ensure the legend correctly lists all scenarios by removing duplicates
handles, labels = plt.gca().get_legend_handles_labels()
by_label = dict(zip(labels, handles))
plt.legend(by_label.values(), by_label.keys(), title='Scenario', bbox_to_anchor=(1.05, 1), loc='upper left')

# Final customizations
plt.xlabel('Category')
plt.ylabel('Value')
plt.title('Bar Graph of Categories by Scenario')
plt.xticks(np.arange(len(unique_x)), unique_x, rotation=45)
plt.tight_layout()
plt.grid(False)

# Display the graph
plt.show()
