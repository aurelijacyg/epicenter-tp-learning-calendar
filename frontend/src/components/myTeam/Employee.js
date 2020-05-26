import React from 'react';
import { useDrop } from 'react-dnd';
import {
  Avatar, Badge, Cell, Divider, Layout, Tag, Text, Tooltip,
} from 'wix-style-react';
import { useDispatch } from 'react-redux';
import {
  assignGoal, assignPersonalGoal, removeGoal, removePersonalGoal,
} from '../../state/actions/assignGoals';
import s from './styles.scss';
import { TOPIC } from '../../constants/DraggableTypes';

const renderLimit = limit => {
  const remainingDays = limit.learningDaysPerQuarter - limit.createdLearningDaysThisQuarter;
  const badgeText = `${remainingDays}/${limit.learningDaysPerQuarter}`;
  const tooltipText = `${remainingDays} out of ${limit.learningDaysPerQuarter} days left`;
  return (
    <Tooltip content={tooltipText}>
      <Badge onClick={() => alert('Badge Clicked')}>
        {badgeText}
      </Badge>
    </Tooltip>
  );
};

const Employee = ({ employee }) => {
  const dispatch = useDispatch();

  const { isSelf } = employee;
  const handleAssignGoal = topic => {
    if (isSelf)
      dispatch(assignPersonalGoal({ topic }));
    else
      dispatch(assignGoal({ employeeId: employee.id, topic }));
  };
  const handleRemoveGoal = topicId => {
    if (isSelf)
      dispatch(removePersonalGoal({ topicId }));
    else
      dispatch(removeGoal({ employeeId: employee.id, topicId }));
  };

  const [{ canDrop, isOver }, drop] = useDrop({
    accept: TOPIC,
    drop: item => handleAssignGoal({ topicId: item.topic.id, topic: item.topic.subject }),
    canDrop: item => !employee.goalTopics.some(goalTopic => goalTopic.topicId === item.topic.id),
    collect: monitor => ({
      isOver: monitor.isOver(),
      canDrop: monitor.canDrop(),
    }),
  });

  const isActive = canDrop && isOver;

  let employeeClass = s.employee;
  if (isActive)
    employeeClass = s.activeEmployee;
  else if (!canDrop && isOver)
    employeeClass = s.notDroppableEmployee;

  if (isSelf)
    employeeClass = `${employeeClass} ${s.selfEmployee}`;

  const hasGoals = Array.isArray(employee.goalTopics) && employee.goalTopics.length > 0;

  return (
    <div className={employeeClass} ref={drop}>
      <Layout gap={8}>
        <Cell span={12}>
          <Layout alignItems="center" gap={0}>
            <Cell span={1}>
              <Avatar
                name={employee.name}
                color={isSelf ? 'A1' : 'A2'}
                size="size36"
              />
            </Cell>
            <Cell span={10}>
              <Text weight={isSelf ? 'bold' : 'normal'}>
                {employee.name}
              </Text>
            </Cell>
            <Cell span={1} vertical>
              {!isSelf && renderLimit(employee.limit)}
            </Cell>
          </Layout>
        </Cell>
        {hasGoals && (
          <Cell span={12}>
            <Divider />
          </Cell>
        )}
        {hasGoals && (
          <Cell span={12}>
            <div className={s.tagContainer}>
              {employee.goalTopics.map(goalTopic => (
                <Tag
                  id={goalTopic.topicId}
                  key={goalTopic.topicId}
                  className={s.topicTag}
                  theme="dark"
                  removable={!!goalTopic.isRemovable}
                  onRemove={() => handleRemoveGoal(goalTopic.topicId)}
                >
                  {goalTopic.topic}
                </Tag>
              ))}
            </div>
          </Cell>
        )}
      </Layout>
    </div>
  );
};

export default Employee;
