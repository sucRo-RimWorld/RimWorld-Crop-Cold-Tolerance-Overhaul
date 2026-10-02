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
