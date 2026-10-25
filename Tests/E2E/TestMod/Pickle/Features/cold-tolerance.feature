@quickstart:CctoColdToleranceQuickstart
Feature: CCTO cold tolerance runtime behavior

  Scenario: Fixed death threshold survives safely above the threshold and dies well below it
    Given a fixed-death CCTO test plant with minimum growth 0 and death -10
    When I set the CCTO test outdoor temperature to 10
    And I run one CCTO plant long tick
    Then the CCTO test plant is alive
    When I set the CCTO test outdoor temperature to -30
    And I run one CCTO plant long tick
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
    And I run one CCTO plant long tick
    Then the CCTO test plant is dormant

  Scenario: A dormant plant can still die below an explicit extreme-cold threshold
    Given a dormancy CCTO test plant with minimum growth 5 and death -30
    When I set the CCTO test outdoor temperature to -10
    And I run one CCTO plant long tick
    Then the CCTO test plant is dormant
    When I set the CCTO test outdoor temperature to -50
    And I run one CCTO plant long tick
    Then the CCTO test plant is dead

  Scenario: Dormancy recovery preserves the vanilla delayed leafless window
    Given a dormancy CCTO test plant with minimum growth 5
    When I set the CCTO test outdoor temperature to -10
    And I run one CCTO plant long tick
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


  Scenario: Cumulative frost lowers visible HP but warming does not heal it
    Given a cumulative CCTO test plant with minimum growth 0 onset -5 dose 20
    When I set the CCTO test outdoor temperature to -10
    And I force the CCTO plant cold check
    Then the CCTO plant HP is recorded at first observation
    When I advance 4000 complete RimWorld game ticks
    Then the CCTO plant HP decreased from last observation
    When I retain a fractional CCTO cold HP remainder
    And I set the CCTO test outdoor temperature to 10
    And I advance 4000 complete RimWorld game ticks
    Then the CCTO plant HP did not recover
    When I set the CCTO test outdoor temperature to -10
    And I simulate 1000 ticks between CCTO damage observations
    Then the CCTO plant HP did not recover
    When I simulate 1000 ticks between CCTO damage observations
    Then the CCTO plant HP decreased from last observation
    And the CCTO test plant is alive

  Scenario: Cumulative frost and dormancy coexist until fatal accumulated exposure
    Given a dormant cumulative CCTO test plant with minimum growth 5 onset -5 dose 5
    When I set the CCTO test outdoor temperature to -10
    And I force the CCTO plant cold check
    Then the CCTO test plant is dormant
    And the CCTO plant HP is recorded at first observation
    When I simulate 2000 ticks between CCTO damage observations
    Then the CCTO plant HP decreased from last observation
    And the CCTO test plant is dormant
    When I simulate 1000 ticks between CCTO damage observations
    Then the CCTO test plant is dead

  # Small, ordinary-Tick native V3 acceptance checks. Do not substitute
  # these for seasonal production-economy or world-climate balance judgments.
  Scenario: Finalized production potato loses visible HP under frost and never cold-heals
    Given an unmodified V3 production potato plant
    When I set the CCTO test outdoor temperature to -10
    And I force the CCTO plant cold check
    Then the production CCTO plant HP is recorded
    When I advance 4000 complete RimWorld game ticks
    Then the CCTO plant HP decreased from last observation
    When I set the CCTO test outdoor temperature to 10
    And I advance 4000 complete RimWorld game ticks
    Then the CCTO plant HP did not recover
    And the CCTO test plant is alive

  Scenario: Finalized production healroot goes dormant rather than dying
    Given an unmodified V3 production healroot plant
    When I set the CCTO test outdoor temperature to -10
    And I run one CCTO plant long tick
    Then the CCTO test plant is dormant
    And the CCTO test plant is alive

  Scenario: Finalized production pine survives an ordinary cold winter observation
    Given an unmodified V3 production pine plant
    When I set the CCTO test outdoor temperature to -35
    And I force the CCTO plant cold check
    Then the production CCTO plant HP is recorded
    When I advance 4000 complete RimWorld game ticks
    Then the production CCTO plant HP is unchanged
    And the CCTO test plant is alive
