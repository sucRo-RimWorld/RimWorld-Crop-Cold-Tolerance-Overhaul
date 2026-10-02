@quickstart:CctoColdToleranceQuickstart
Feature: CCTO cold tolerance runtime behavior

  Scenario: Fixed death threshold survives safely above the threshold and dies well below it
    Given a fixed-death CCTO test plant with minimum growth 0 and death -10
    When I set the CCTO test outdoor temperature to 10
    And I wait 2100 ticks
    Then the CCTO test plant is alive
    When I set the CCTO test outdoor temperature to -30
    And I wait 2100 ticks
    Then the CCTO test plant is dead

  Scenario: Fixed death threshold uses a strict below-threshold boundary
    Given a fixed-death CCTO test plant with minimum growth 0 and death -10
    When I set the CCTO test outdoor temperature to -10
    And I force the CCTO plant cold check
    Then the CCTO test plant is alive
    When I set the CCTO test outdoor temperature to -10.5
    And I force the CCTO plant cold check
    Then the CCTO test plant is dead

  Scenario: Dormancy keeps a plant alive below its minimum growth temperature
    Given a dormancy CCTO test plant with minimum growth 5
    When I set the CCTO test outdoor temperature to -10
    And I wait 2100 ticks
    Then the CCTO test plant is dormant

  Scenario: A dormant plant can still die below an explicit extreme-cold threshold
    Given a dormancy CCTO test plant with minimum growth 5 and death -30
    When I set the CCTO test outdoor temperature to -10
    And I wait 2100 ticks
    Then the CCTO test plant is dormant
    When I set the CCTO test outdoor temperature to -50
    And I wait 2100 ticks
    Then the CCTO test plant is dead

  Scenario: Dormancy recovery preserves the vanilla delayed leafless window
    Given a dormancy CCTO test plant with minimum growth 5
    When I set the CCTO test outdoor temperature to -10
    And I wait 2100 ticks
    Then the CCTO test plant is dormant
    When I remember the CCTO dormancy refresh tick
    And I set the CCTO test outdoor temperature to 10
    And I force the CCTO plant cold check
    Then the CCTO test plant is dormant
    And the CCTO dormancy refresh tick has not changed
    When I expire the CCTO dormancy recovery timer
    Then the CCTO test plant has recovered from dormancy

  Scenario: Indoor plants use actual room temperature rather than outdoor temperature
    Given a fixed-death CCTO test plant with minimum growth 0 and death -10
    When I enclose the CCTO test plant in a roofed indoor room
    And I set only the CCTO test outdoor temperature to -30
    And I set the CCTO test plant room temperature to 10
    And I force the CCTO plant cold check
    Then the CCTO test plant is alive
    When I set the CCTO test plant room temperature to -30
    And I force the CCTO plant cold check
    Then the CCTO test plant is dead
