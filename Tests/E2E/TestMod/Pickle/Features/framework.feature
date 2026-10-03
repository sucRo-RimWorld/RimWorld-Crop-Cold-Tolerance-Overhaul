Feature: CCTO framework regression behavior

  Scenario: Fixed threshold behavior does not vary by plant identity
    Then CCTO fixed death thresholds are independent of plant identity

  Scenario: Dormancy threshold follows native growth threshold
    Then CCTO dormancy uses the plant minimum growth temperature as its cold threshold

  Scenario: Unsupported plants keep vanilla cold threshold behavior
    Then an unconfigured plant retains the vanilla per-plant cold threshold range

  Scenario: Extension configuration validation remains stable
    Then CCTO extension validation accepts dormancy-only and rejects incomplete ordinary configuration

  Scenario: Info Card entries match the configured low-temperature behavior
    Then CCTO Info Card stat construction matches fixed-death and dormancy configuration
