import React from 'react';
import { Text, StatusIndicator, InfoIcon } from 'wix-style-react';
import s from './LimitsCard.scss';

const LimitsCard = ({ assignedLimit, remainingLimit, isLoading }) => {
  let remainingDaysPerQuarterClassName;
  if (!isLoading) {
    if (remainingLimit.daysPerQuarter <= 0)
      remainingDaysPerQuarterClassName = s.rightRedText;
    else if (remainingLimit.daysPerQuarter === 1)
      remainingDaysPerQuarterClassName = s.rightYellowText;
    else
      remainingDaysPerQuarterClassName = s.rightLimitText;
  }

  return (
    <div>
      <div style={{ marginBottom: 6, textAlign: 'left' }}>
        <Text size="medium" weight="normal">LIMITS:</Text>
        {isLoading ? <StatusIndicator className={s.rightText} status="loading" message="Loading limits" />
          : (
            <span className={s.rightText}>
              days per quarter:
              {' '}
              {assignedLimit.daysPerQuarter}
              {' '}
              | topics per day:
              {' '}
              {assignedLimit.topicsPerDay}
            </span>
          )}
      </div>
      <div style={{ marginBottom: 12, textAlign: 'left' }}>
        <Text size="medium" weight="normal">LEFT:</Text>
        {isLoading ? <StatusIndicator className={s.rightText} status="loading" message="Loading limits" />
          : (
            <>
              {remainingLimit.daysPerQuarter < 0 && (
              <span className={s.infoIcon}>
                <InfoIcon content="Someone decreased your limit. You now have more learning days than you should." />
              </span>
              )}
              <span className={remainingDaysPerQuarterClassName}>
                {remainingLimit.daysPerQuarter}
              </span>
              <span className={s.rightText}>
                days per this quarter:
              </span>
            </>
          )}
      </div>
    </div>
  );
};

export default LimitsCard;
