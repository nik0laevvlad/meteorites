import { useEffect, useState } from 'react';
import { http } from '../http';
import { useNotify } from '../components';
import { Formik, Form as FormikForm } from 'formik';
import { Button, DatePicker, Input, Select, Form } from 'antd';
import type dayjs from 'dayjs';

export interface FilterValue {
  yearRange?: [dayjs.Dayjs, dayjs.Dayjs];
  recClass?: string;
  name?: string;
}

const initialValues: FilterValue = {
  yearRange: null!,
  recClass: null!,
  name: null!,
};

interface Props {
  onSubmit: (value: FilterValue) => void;
}

export const MeteoriteFilterForm = (props: Props) => {
  const { onSubmit } = props;
  const notify = useNotify();
  const [classes, setClasses] = useState<string[]>([]);

  const fetchClasses = async () => {
    await http
      .getClasses()
      .then((v) => setClasses(v))
      .catch((e) => notify.error({ message: 'Error', description: e.message }));
  };

  useEffect(() => {
    fetchClasses();
  }, []);

  return (
    <Formik initialValues={initialValues} onSubmit={onSubmit}>
      {({ setFieldValue, values }) => (
        <FormikForm>
          <Form.Item label="Year">
            <DatePicker.RangePicker
              value={values.yearRange}
              picker="year"
              onChange={(dates) => setFieldValue('yearRange', dates)}
            />
          </Form.Item>

          <Form.Item label="Class">
            <Select
              allowClear
              style={{ width: 200 }}
              value={values.recClass}
              onChange={(val) => setFieldValue('recClass', val)}
            >
              {classes.map((c) => (
                <Select.Option key={c} value={c}>
                  {c}
                </Select.Option>
              ))}
            </Select>
          </Form.Item>

          <Form.Item label="Name">
            <Input
              value={values.name}
              onChange={(e) => setFieldValue('name', e.target.value)}
              placeholder="Search by name"
            />
          </Form.Item>

          <Button type="primary" htmlType="submit" style={{ marginTop: 4 }}>
            Search
          </Button>
        </FormikForm>
      )}
    </Formik>
  );
};
