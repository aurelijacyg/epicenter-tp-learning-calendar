import React from 'react';
import './SignIn.global.scss';
import {
  Button,
  Box,
  Layout,
  Image,
  Heading,
  InputWithLabel,
  Cell,
  Row,
  Col,
  Container,
  Card,
} from 'wix-style-react';
import { Route } from 'react-router-dom';

const SignIn = ({ onLogin }) => {
  const logo = require('../../assets/logo.jpeg');
  const handleSignInBtn = history => {
    onLogin();
    history.push('/home');
  };

  return (
    <div className="login">
      <Layout cols={1}>
        <Box align="center" marginTop="10%">
          <Layout cols={1}>
            <Image src={logo} height="200px" width="200px" />
            <Heading appearance="H1" className="header">
              The Learning Center
            </Heading>
            <Heading appearance="H3"> Log in to get going</Heading>
          </Layout>
        </Box>
        <Container>
          <Row>
            <Col span="3" />
            <Col span="6">
              <Card>
                <Card.Content>
                  <Container fluid>
                    <Row>
                      <Col>
                        <Cell>
                          <InputWithLabel label="Email" />
                        </Cell>
                      </Col>
                    </Row>
                    <Row>
                      <Col>
                        <Cell>
                          <InputWithLabel type="password" label="Password" />
                        </Cell>
                      </Col>
                    </Row>
                    <Row>
                      <Col>
                        <Box align="right">
                          <Route
                            render={({ history }) => (
                              <Button
                                as="button"
                                onClick={() => handleSignInBtn(history)}
                              >
                                Login
                              </Button>
                            )}
                          />
                        </Box>
                      </Col>
                    </Row>
                  </Container>
                </Card.Content>
              </Card>
            </Col>
          </Row>
        </Container>
      </Layout>
    </div>
  );
};
export default SignIn;