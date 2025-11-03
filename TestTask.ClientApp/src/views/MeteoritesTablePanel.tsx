import { Table } from 'antd';
import { MeteoriteFilterForm, type FilterValue } from './MeteoritesFilterForm';
import { useCallback, useEffect, useMemo, useState } from 'react';
import { http, type FilterValueRequest, type MeteoriteSummary } from '../http';
import { useNotify } from '../components';

export const MeteoritesTablePanel = () => {
  const notify = useNotify();
  const [data, setData] = useState<MeteoriteSummary[]>([]);
  const [filters, setFilters] = useState<FilterValueRequest>({});
  const [sort, setSort] = useState<{
    sortField?: string;
    sortOrder?: 'asc' | 'desc';
  }>({});

  const fetchData = useCallback(
    async (newFilters: FilterValueRequest) => {
      setFilters(newFilters);
      try {
        const res = await http.getData(newFilters);
        setData(res);
      } catch (e: any) {
        notify.error({ message: 'Error', description: e.message });
      }
    },
    [notify],
  );

  useEffect(() => {
    fetchData({});
  }, [fetchData]);

  const submit = useCallback(
    (values: FilterValue) => {
      const filter = {
        yearFrom: values.yearRange?.[0]?.toISOString(),
        yearTo: values.yearRange?.[1]?.toISOString(),
        recClass: values.recClass,
        name: values.name,
      };
      fetchData({ ...filter, ...sort });
    },
    [fetchData, sort],
  );

  const sum = useMemo(
    () =>
      data.reduce(
        (acc, x) => {
          acc.count += x.count;
          acc.mass += x.totalMass;
          return acc;
        },
        { count: 0, mass: 0 },
      ),
    [data],
  );

  const handleTableChange = (_p: any, _f: any, sorter: any) => {
    const sortField = sorter.field;
    const sortOrder =
      sorter.order === 'ascend'
        ? 'asc'
        : sorter.order === 'descend'
        ? 'desc'
        : undefined;

    setSort({ sortField, sortOrder });
    fetchData({ ...filters, sortField, sortOrder });
  };

  const columns = [
    { title: 'Year', dataIndex: 'year', key: 'year', sorter: true },
    { title: 'Count', dataIndex: 'count', key: 'count', sorter: true },
    {
      title: 'Total mass',
      dataIndex: 'totalMass',
      key: 'totalMass',
      sorter: true,
    },
  ];

  return (
    <>
      <MeteoriteFilterForm onSubmit={submit} />
      <Table
        className="mt-2"
        columns={columns}
        dataSource={data}
        pagination={false}
        scroll={{ y: 350 }}
        onChange={handleTableChange}
        summary={() => (
          <Table.Summary fixed>
            <Table.Summary.Row>
              <Table.Summary.Cell index={0}>
                <b>Total</b>
              </Table.Summary.Cell>
              <Table.Summary.Cell index={1}>
                <b>{sum.count}</b>
              </Table.Summary.Cell>
              <Table.Summary.Cell index={2}>
                <b>{sum.mass}</b>
              </Table.Summary.Cell>
            </Table.Summary.Row>
          </Table.Summary>
        )}
      />
    </>
  );
};
